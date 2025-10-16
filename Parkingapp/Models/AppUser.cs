using Microsoft.AspNetCore.Identity;

namespace ParkingApp.Models
{
     public class AppUser : IdentityUser
    {
        public ICollection<Car> Cars { get; set; } = new List<Car>();
        public Account Account { get; set; }
    }
}