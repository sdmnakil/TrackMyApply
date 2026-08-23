using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Auth;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);

    Task<string?> LoginAsync(LoginDto dto);
}