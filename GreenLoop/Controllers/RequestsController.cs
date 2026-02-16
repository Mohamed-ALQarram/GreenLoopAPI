using GreenLoop.BLL.DTOs;
using GreenLoop.BLL.Interfaces.IServices;
using GreenLoop.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreenLoop.Controllers
{
    [ApiController]
    [Route("api/requests")]
    //[Authorize]
    [AllowAnonymous]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        // ─── Helper: Extract UserId from JWT Claims ─────────────────────
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User ID not found in token.");
            return int.Parse(userIdClaim);
        }

        // ─── GET /api/requests/waste-categories ─────────────────────────
        [HttpGet("waste-categories")]
        public async Task<IActionResult> GetWasteCategories()
        {
            var categories = await _requestService.GetWasteCategoriesAsync();
            return Ok(categories);
        }

        // ─── POST /api/requests/waste-categories ────────────────────────
        [HttpPost("waste-categories")]
        public async Task<IActionResult> AddWasteCategory([FromBody] CreateWasteCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _requestService.AddWasteCategoryAsync(dto);
            return CreatedAtAction(nameof(GetWasteCategories), new { id = created.Id }, created);
        }

        // ─── POST /api/requests/schedule ────────────────────────────────
        [HttpPost("schedule")]
        public async Task<IActionResult> SchedulePickup([FromBody] SchedulePickupRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetCurrentUserId();
                var requestId = await _requestService.SchedulePickupAsync(userId, dto);

                return CreatedAtAction(
                    nameof(GetRequestDetails),
                    new { id = requestId },
                    new { id = requestId, message = "Pickup request scheduled successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ─── GET /api/requests/history ──────────────────────────────────
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(
            [FromQuery] int page = 1,
            [FromQuery] RequestStatus? status = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _requestService.GetRequestHistoryAsync(userId, page, status);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ─── GET /api/requests/{id} ─────────────────────────────────────
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRequestDetails(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _requestService.GetRequestDetailsAsync(userId, id);

            if (result == null)
                return NotFound(new { error = "Request not found or access denied." });

            return Ok(result);
        }

        // ─── POST /api/requests/{id}/cancel ─────────────────────────────
        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> CancelRequest(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _requestService.CancelRequestAsync(userId, id);

                if (!success)
                    return NotFound(new { error = "Request not found." });

                return Ok(new { message = "Request cancelled successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
