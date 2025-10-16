using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingApp.Endpoints;
using ParkingApp.Models;
using System.Security.Claims;

namespace NewParkingApp.Controllers
{
    [ApiController]
    [Route("cars")]
    public class CarController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;


        public CarController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCar(NewCarDto newCar)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.Cars)
                .ThenInclude(c => c.Periods)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return BadRequest( new { error = "User not found" });

            if (user.Cars.Any(c => c.Numberplate == newCar.Numberplate.ToUpper()))
                return BadRequest(new { error =  "A car with that numberplate already exists." });

            var car = new Car(newCar.Numberplate.ToUpper());
            user.Cars.Add(car);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "Car added successfully", numberplate = car.Numberplate });

            return BadRequest(new { result.Errors });
        }

        [Authorize]
        [HttpDelete("{numberplate}")]
        public async Task<IActionResult> RemoveCar(string numberplate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.Cars)
                .ThenInclude(c => c.Periods)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return BadRequest(new { error = "User not found" });

            var car = user.Cars.FirstOrDefault(c => c.Numberplate == numberplate);
            if (car == null)
                return BadRequest(new { error = "Car not found" });

            user.Cars.Remove(car);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "Car removed successfully" });

            return BadRequest(new { error = result.Errors });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.Users
                .Include(u => u.Cars)
                .ThenInclude(c => c.Periods)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return BadRequest(new { error = "User not found" });

            var cars = user.Cars.Select(c => new
            {
                c.Id,
                c.Numberplate,
                active = c.Periods.FirstOrDefault(p => p.EndTime == null) != null
            }).ToArray();

            return Ok(cars);
        }
    }
}
