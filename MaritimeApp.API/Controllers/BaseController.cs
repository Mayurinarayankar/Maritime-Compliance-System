using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MaritimeApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
        }
    }

    protected string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

    protected bool IsAdmin => CurrentUserRole == "Admin";
}