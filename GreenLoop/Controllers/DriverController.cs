using GreenLoop.BLL.DTOs;
using GreenLoop.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreenLoop.Controllers
{
    [Route("api/driver")]
    [ApiController]
    //[Authorize(Roles = "Driver")] // Assuming role based auth, can uncomment later if needed
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _service;

        public DriverController(IDriverService service)
        {
            _service = service;
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] DriverStatusDto dto)
        {
            var driverId = GetCurrentUserId(); 
            if (driverId == 0) return Unauthorized();

            var result = await _service.UpdateStatusAsync(driverId, dto.Status);
            if (!result) return BadRequest("Failed to update status.");
            
            return Ok(new { message = "Status updated successfully." });
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks([FromQuery] string? date, [FromQuery] string? status)
        {
            var driverId = GetCurrentUserId();
            if (driverId == 0) return Unauthorized();

            DateTime targetDate = DateTime.Today;
            if (date?.ToLower() == "today")
            {
                 targetDate = DateTime.Today;
            }
            else if (DateTime.TryParse(date, out var parsedDate))
            {
                targetDate = parsedDate;
            }

            // status is not used in service call? 
            // Service method is GetAssignedTasksAsync(driverId, date) which uses RequestStatus.Assigned inside.
            // Requirement says "status=Assigned".
            
            var tasks = await _service.GetAssignedTasksAsync(driverId, targetDate);
            return Ok(tasks);
        }

        [HttpPost("tasks/{id}/start")]
        public async Task<IActionResult> StartTask(int id)
        {
            var result = await _service.StartTaskAsync(id);
            if (!result) return BadRequest("Failed to start task.");
            return Ok(new { message = "Task started." });
        }

        [HttpPost("tasks/{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id, [FromBody] TaskCompleteDto dto)
        {
            var points = await _service.CompleteTaskAsync(id, dto);
            if (points == null) return BadRequest("Failed to complete task.");
            
            return Ok(new { points = points });
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
            if (idClaim != null && int.TryParse(idClaim.Value, out int id))
            {
                return id;
            }
            return 1; // Testing fallback, should return 0 in production
        }
    }
}
