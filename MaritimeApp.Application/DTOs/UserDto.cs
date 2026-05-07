namespace MaritimeApp.Application.DTOs
{
   public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Rank { get; set; } = string.Empty;
    public Guid? ShipId { get; set; }
    public string? ShipName { get; set; }
}
}