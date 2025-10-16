using ParkingApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Endpoints
{
    public record NewPeriodDto(
        [Required]Car Car
        );
}
