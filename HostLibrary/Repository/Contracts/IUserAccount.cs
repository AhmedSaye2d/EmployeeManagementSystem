using BaseLibrary.DTOs;
using BaseLibrary.Respones;
namespace HostLibrary.Repository.Contracts
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAsync(Register register);
        Task<LoginResponse> LoginAsync(Login login);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
    }
}
