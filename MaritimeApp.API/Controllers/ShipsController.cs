using MaritimeApp.Application.DTOs;
using MaritimeApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaritimeApp.API.Controllers;

[Authorize]
public class ShipsController : BaseController
{
    private readonly IShipService _service;

    public ShipsController(IShipService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllShipsAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var ship = await _service.GetShipByIdAsync(id);
        return ship == null ? NotFound() : Ok(ship);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateShipDto dto)
    {
        var ship = await _service.CreateShipAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = ship.Id }, ship);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShipDto dto)
    {
        var ship = await _service.UpdateShipAsync(id, dto);
        return ship == null ? NotFound() : Ok(ship);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteShipAsync(id);
        return result ? NoContent() : NotFound();
    }
}