using GreenLoop.BLL.DTOs;
using GreenLoop.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreenLoop.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    //[Authorize(Roles = "Customer")] // Uncomment when auth is ready
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _service;

        public WalletController(IWalletService service)
        {
            _service = service;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var balance = await _service.GetBalanceAsync(userId);
            return Ok(balance);
        }

        [HttpGet("coupons")]
        public async Task<IActionResult> GetCoupons()
        {
             var userId = GetCurrentUserId();
             // Not technically needed for listing coupons, but good for tracking who views them if needed later
             
             var coupons = await _service.GetAvailableCouponsAsync(userId);
             return Ok(coupons);
        }

        [HttpPost("redeem")]
        public async Task<IActionResult> RedeemCoupon([FromBody] RedeemRequestDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var result = await _service.RedeemCouponAsync(userId, dto);
            if (result == null) return BadRequest("Redemption failed. Insufficient funds, invalid coupon, or user not found.");

            return Ok(result);
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] int page = 1)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            const int pageSize = 10;
            var transactions = await _service.GetTransactionsAsync(userId, page, pageSize);
            return Ok(transactions);
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
            if (idClaim != null && int.TryParse(idClaim.Value, out int id))
            {
                return id;
            }
            return 1; // Testing fallback
        }
    }
}
