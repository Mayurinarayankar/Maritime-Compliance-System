using MaritimeApp.Application.DTOs;
using MaritimeApp.Application.Interfaces;
using MaritimeApp.Application.IRepositories;
using MaritimeApp.Domain.Entities;
using MaritimeApp.Domain.Enums;

namespace MaritimeApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IShipRepository _shipRepo;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepo, IShipRepository shipRepo, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _shipRepo = shipRepo;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email.ToLower());
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated.");

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());
        var userDto = await MapUserDtoAsync(user);

        return new AuthResponseDto { Token = token, User = userDto };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existing = await _userRepo.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new InvalidOperationException("Email already in use.");

        if (!Enum.TryParse<UserRole>(dto.Role, true, out var role))
            role = UserRole.Crew;

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email.ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = role,
            Rank = dto.Rank,
            ShipId = dto.ShipId,
            IsActive = true
        };

        await _userRepo.AddAsync(user);

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());
        var userDto = await MapUserDtoAsync(user);

        return new AuthResponseDto { Token = token, User = userDto };
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepo.GetWithDetailsAsync(id);
        return user == null ? null : await MapUserDtoAsync(user);
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepo.GetAllAsync();
        var dtos = new List<UserDto>();
        foreach (var u in users) dtos.Add(await MapUserDtoAsync(u));
        return dtos;
    }

    public async Task<List<UserDto>> GetCrewByShipAsync(Guid shipId)
    {
        var users = await _userRepo.GetByShipAsync(shipId);
        var dtos = new List<UserDto>();
        foreach (var u in users) dtos.Add(await MapUserDtoAsync(u));
        return dtos;
    }

    private async Task<UserDto> MapUserDtoAsync(User user)
    {
        string? shipName = null;
        if (user.ShipId.HasValue)
        {
            var ship = user.Ship ?? await _shipRepo.GetByIdAsync(user.ShipId.Value);
            shipName = ship?.Name;
        }

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Role = user.Role.ToString(),
            Rank = user.Rank,
            ShipId = user.ShipId,
            ShipName = shipName
        };
    }
}