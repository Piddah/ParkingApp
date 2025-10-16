using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Endpoints
{
    public record LoginUserDto(
        [EmailAddress] string Email,
        [Required][Length(6, 30)] string Password
     );
}
