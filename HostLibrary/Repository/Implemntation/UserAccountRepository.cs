using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Respones;
using HostLibrary.Data;
using HostLibrary.Helpers;
using HostLibrary.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace HostLibrary.Repository.Implemntation
{
    public class UserAccountRepository : IUserAccount
    {
        private readonly AppDbContext _context;
        private readonly JwtSection _config;

        public UserAccountRepository(AppDbContext context, IOptions<JwtSection> config)
        {
            _context = context;
            _config = config.Value;
        }
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if (user == null)
            {
                return new GeneralResponse(false, "User data is null", null);
            }

            var existingUser = await FindUserByEmail(user.Email);

            if (existingUser != null)
            {
                return new GeneralResponse(false, " User with this email already exists", null);
            }

            var appuser = await AddToDatabase(new ApplicationUser()
            {
                FullName = user.FullName!,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            });

            var checkaadminrole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.Admin));

            if (checkaadminrole is null)
            {
                var createAdminRole = await AddToDatabase(new SystemRole { Name = Constants.Admin });
                await AddToDatabase(new UserRole()
                {
                    RoleId = createAdminRole.Id,
                    UserId = appuser.Id
                });
                return new GeneralResponse(true, "Admin role created and assigned to the user");
            }

            var checkuserrole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.User));
            SystemRole response = new();
            if (checkuserrole is null)
            {
                response = await AddToDatabase(new SystemRole { Name = Constants.User });
                await AddToDatabase(new UserRole()
                {
                    RoleId = response.Id,
                    UserId = appuser.Id
                });
            }
            else
            {
                await AddToDatabase(new UserRole()
                {
                    RoleId = checkuserrole.Id,
                    UserId = appuser.Id
                });
            }
            return new GeneralResponse(true, "User created successfully", null);

        }

        public async Task<LoginResponse> LoginAsync(Login user)
        {
            if (user is null)
            {
                return new LoginResponse(false, "Login data is null", null);
            }

            var existingUser = await FindUserByEmail(user.Email);

            if (existingUser is null)
            {
                return new LoginResponse(false, "Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                return new LoginResponse(false, "Invalid email or password", null);
            }
            var getuserroles = await FindUserRoleByUserId(existingUser.Id);

            if (getuserroles is null)
            {
                return new LoginResponse(false, "User has no assigned roles");
            }
            //get the role name from the role id

            var getrolename = await FindRoleName(getuserroles.RoleId);

            string token = GenerateJwtToken(existingUser, getrolename.Name);
            string refreshToken =  GenerateRefreshToken();
            await AddToDatabase(new RefreshTokenInfo
            {
                UserId = existingUser.Id,
                Token = refreshToken
            });
            return new LoginResponse(true, "Login successful", token, refreshToken);
        }
        private string GenerateJwtToken(ApplicationUser user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role,role!)

            };
            var token = new JwtSecurityToken(
                  issuer: _config.Issuer,
                  audience: _config.Audience,
                  claims: claims,
                  expires: DateTime.Now.AddMinutes(_config.ExpiryInMinutes),
                  signingCredentials: credentials
              );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<UserRole> FindUserRoleByUserId(int userId)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == userId);
        }
        private async Task<SystemRole> FindRoleName(int roleId)
        {
            return await _context.SystemRoles.FirstOrDefaultAsync(r => r.Id == roleId);
        }
        private string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<ApplicationUser> FindUserByEmail(string email)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower()!.Equals(email.ToLower()!));
        }
        private async Task<T> AddToDatabase<T>(T model)
        {
            var entry = await _context.AddAsync(model!);
            await _context.SaveChangesAsync();
            return (T)entry.Entity;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            if (token is null) return new LoginResponse(false, "Invalid refresh token");

            var findtoken = await _context.RefreshTokenInfos.FirstOrDefaultAsync(t => t.Token.Equals(token.Token));

            if (findtoken is null) return new LoginResponse(false, "Refresh token not found");

            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == findtoken.UserId);

            if (user is null) return new LoginResponse(false, "User not found for the provided refresh token");

            var userrole = await FindUserRoleByUserId(user.Id);
            var role = await FindRoleName(userrole.RoleId);
            string newToken = GenerateJwtToken(user, role.Name);
            string newRefreshToken =  GenerateRefreshToken();

            var updaterefreshToken = await _context.RefreshTokenInfos.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (updaterefreshToken is not null)
            {
                updaterefreshToken.Token = newRefreshToken;
            }
            else
            {
                await _context.RefreshTokenInfos.AddAsync(new RefreshTokenInfo
                {
                    UserId = user.Id,
                    Token = newRefreshToken
                });
            }

            await _context.SaveChangesAsync();
            return new LoginResponse(true, "Token refreshed successfully", newToken, newRefreshToken);

        }
    }
}
