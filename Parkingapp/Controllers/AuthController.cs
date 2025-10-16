using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewParkingApp.Dtos;
using ParkingApp.Endpoints;
using ParkingApp.Models;
using ParkingApp.Services;
using ParkingApp.Data;


namespace NewParkingApp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly ParkingContext _context;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService, ParkingContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(NewUserDto newUser)
        {
            if (newUser.Password != newUser.ConfirmPassword)
                return BadRequest(new { error = "Passwords do not match" });

            var existingUser = await _userManager.FindByEmailAsync(newUser.Email);
            if (existingUser != null)
                return BadRequest(new { error = "A user with that email already exists" });

            var user = new AppUser { UserName = newUser.Email, Email = newUser.Email };
            var result = await _userManager.CreateAsync(user, newUser.Password);

            if (result.Succeeded)
            {
                // Create linked account for user
                var account = new Account { UserId = user.Id };
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User created successfully" });
            }

            
            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUser)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if (user != null)
                {
                    var token = _tokenService.GenerateJwtToken(user);

                    return Ok(new
                    {
                        userId = user.Id,
                        token
                    });
                }
            }

            return BadRequest(new { error = "Invalid login attempt" });
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users
                .Include(u => u.Cars)
                .Include(u => u.Account)
                .ToListAsync();

            var userList = new List<object>();

            foreach (var user in users)
            {
                userList.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.Cars,
                    user.Account
                });
            }

            return Ok(userList);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "User logged out successfully" });
        }




    }
}
