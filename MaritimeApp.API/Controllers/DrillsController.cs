// using MaritimeApp.Application.DTOs;
// using MaritimeApp.Application.Services;
// using Microsoft.AspNetCore.Mvc;

// namespace MaritimeApp.API.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class DrillsController : ControllerBase
// {
//     private readonly IDrillService _service;

//     public DrillsController(IDrillService service)
//     {
//         _service = service;
//     }

//     [HttpPost]
//     public async Task<IActionResult> Schedule(CreateDrillDto dto)
//     {
//         await _service.ScheduleDrill(dto);
//         return Ok("Drill scheduled");
//     }

//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var drills = await _service.GetAllDrills();
//         return Ok(drills);
//     }

//     [HttpPost("attendance")]
//     public async Task<IActionResult> MarkAttendance(DrillAttendanceDto dto)
//     {
//         await _service.MarkAttendance(dto);
//         return Ok("Attendance marked");
//     }
// }