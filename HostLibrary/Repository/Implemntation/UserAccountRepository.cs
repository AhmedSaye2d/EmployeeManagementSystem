using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Respones;
using HostLibrary.Data;
using HostLibrary.Helpers;
using HostLibrary.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace HostLibrary.Repository.Implemntation
{
    public class UserAccountRepository : IUserAccount
    {
        private readonly AppDbContext _context;
        private readonly JwtSection _jwtConfig;

        public UserAccountRepository(AppDbContext context, IOptions<JwtSection> config)
        {
            _context = context;
            _jwtConfig = config.Value;
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
               var createAdminRole =await AddToDatabase(new SystemRole { Name = Constants.Admin });
                await AddToDatabase(new UserRole()
                {
                    RoleId = createAdminRole.Id,
                    UserId = appuser.Id
                });
                return new GeneralResponse(true, "Admin role created and assigned to the user");
            }

            var checkuserrole = await _context.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.User));
            SystemRole response = new();
            if(checkuserrole is null)
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

        //public async Task<LoginResponse> LoginAsync(Login login)
        //{

        //}

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

        public Task<LoginResponse> LoginAsync(Login login)
        {
            throw new NotImplementedException();
        }
    }
}
