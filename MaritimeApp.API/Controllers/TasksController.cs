// using Microsoft.AspNetCore.Mvc;
// using MaritimeApp.Application.Services;
// using MaritimeApp.Application.DTOs;

// namespace MaritimeApp.API.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class TasksController : ControllerBase
// {
//     private readonly ITaskService _service;

//     public TasksController(ITaskService service)
//     {
//         _service = service;
//     }

//     [HttpPost]
//     public async Task<IActionResult> Create(CreateTaskDto dto)
//     {
//         await _service.CreateTask(dto);
//         return Ok("Task created");
//     }

//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var data = await _service.GetAllTasks();
//         return Ok(data);
//     }

//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
//     {
//         await _service.UpdateTask(id, dto);
//         return Ok("Task updated");
//     }
// }