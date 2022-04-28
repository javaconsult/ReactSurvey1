using ClassLibrary1.DTOs;

namespace WebApi.Services
{
    public interface IAccountService
    {
        Task<UserDto> GetCurrentUserAsync(string email);
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
    }
}