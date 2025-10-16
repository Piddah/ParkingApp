using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Dtos
{
    public record PaymentDto(
        [Required][Range(0.01, double.MaxValue)] double Amount
    );
}

