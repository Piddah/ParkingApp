using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingApp.Models;
using System.Security.Claims;

namespace NewParkingApp.Controllers
{


    [ApiController]
    [Route("parking")]
    public class ParkingController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public ParkingController( UserManager<AppUser> userManager )
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("start/{numberplate}")]
        public async Task<IActionResult> StartPeriod(string numberplate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.Cars)
                .ThenInclude(c => c.Periods)
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return BadRequest("User not found");

            var car = user.Cars.FirstOrDefault(c => c.Numberplate == numberplate);
            if (car == null)
                return BadRequest("Car not found");

            // ensure no active period exists
            if (car.Periods.Any(p => p.EndTime == null))
                return BadRequest("An active parking session already exists for this car.");

            var period = new Period { CarId = car.Id };
            period.StartPeriod();
            car.Periods.Add(period);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "Period started successfully", numberplate, startTime = period.StartTime });

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPost("end/{numberplate}")]
        public async Task<IActionResult> EndPeriod(string numberplate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.Cars)
                .ThenInclude(c => c.Periods)
                .Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return BadRequest("User not found");

            var car = user.Cars.FirstOrDefault(c => c.Numberplate == numberplate);
            if (car == null)
                return BadRequest("Car not found");

            var active = car.Periods.FirstOrDefault(p => p.EndTime == null);
            if (active == null)
                return BadRequest("No active parking session found.");

            active.EndPeriod();
            var cost = active.CalculateCost();

            if (user.Account == null)
                return BadRequest("Account not found");

            var transaction = new Transaction(cost, DateTime.Now, user.Account);
            user.Account.AddParkingDebt(transaction);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "Period ended successfully", numberplate, startTime = active.StartTime, endTime = active.EndTime, cost });

            return BadRequest(result.Errors);
        }

    }
}
