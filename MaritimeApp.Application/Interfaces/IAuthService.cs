using MaritimeApp.Application.DTOs;

namespace MaritimeApp.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<List<UserDto>> GetCrewByShipAsync(Guid shipId);
}