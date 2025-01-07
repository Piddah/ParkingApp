using ParkingApp.Data;

namespace ParkingApp.Models
{
    public class Car
    {
        public Guid Id { get; private set; }
        public string Numberplate { get; private set; } = string.Empty;


        public Car(string numberplate)
        {
            Id = new Guid();
            Numberplate = numberplate;
        }
    }


}
