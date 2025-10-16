using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingApp.Models;
using System.Security.Claims;
using ParkingApp.Dtos;

namespace NewParkingApp.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.Account == null) return NotFound(new { error = "Account not found" });

            return Ok(new { balance = user.Account.Debt });
        }

        [Authorize]
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.Account)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.Account == null) return NotFound(new { error = "Account not found" });

            var txs = user.Account.Transactions
                .OrderByDescending(t => t.Date)
                .Select(t => new { t.Id, t.Amount, t.Date })
                .ToArray();

            return Ok(txs);
        }

        [Authorize]
        [HttpPost("pay")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentDto dto)
        {
            if (dto == null || dto.Amount <= 0) return BadRequest(new { error = "Invalid amount" });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.Account)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.Account == null) return NotFound(new { error = "Account not found" });

            var payment = new Transaction(dto.Amount, DateTime.Now, user.Account);

            try
            {
                user.Account.MakePayment(payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "Payment recorded", balance = user.Account.Debt });

            return BadRequest(new { error = result.Errors });
        }
    }
}
