using GreenLoop.BLL.DTOs;
using GreenLoop.BLL.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreenLoop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrController : ControllerBase
    {
        private readonly IQRService _qrService;

        public QrController(IQRService qrService)
        {
            _qrService = qrService;
        }

        [HttpPost("scan")]
        // [Authorize(Roles = "Driver")]
        public async Task<IActionResult> ScanQrCode([FromBody] ScanRequestDto request)
        {
            try
            {
                var driverIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Assuming NameIdentifier holds the ID
                if (driverIdClaim == null || !int.TryParse(driverIdClaim.Value, out int driverId))
                {
                    return Unauthorized("Invalid driver identity.");
                }

                var result = await _qrService.ProcessScanAsync(request, driverId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("redeem")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RedeemCoupon([FromBody] RedeemRequestDto request)
        {
             try
            {
                var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
                {
                    return Unauthorized("Invalid customer identity.");
                }

                var result = await _qrService.RedeemCouponAsync(request, customerId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("generate")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GenerateQrToken()
        {
            try
            {
                var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (customerIdClaim == null || !int.TryParse(customerIdClaim.Value, out int customerId))
                {
                    return Unauthorized("Invalid customer identity.");
                }

                var result = await _qrService.GenerateQrTokenAsync(customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
