using ParkingApp.Data;

namespace ParkingApp.Models
{
    public class User
    {
        private static int NextId = 1;
        public int Id { get; private set; }
        public string Firstname { get; private set; } = string.Empty;
        public string Lastname { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;

        public List<Car> Cars { get; private set; }
        public Account Account { get; private set; }

        public User(string firstname, string lastname, string email, string password, List<Car>? cars = null)
        {
            Id = NextId++;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
            Cars = cars ?? new List<Car>();
            Account = new Account();
        }

        public void UpdateUser(string firstname, string lastname, string email, string password)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
        }

        public void AddCar(Car car)
        {
            Cars.Add(car);
        }

        public void RemoveCar(Car car)
        {
            if (!Cars.Remove(car))
                throw new KeyNotFoundException("Car not found.");
        }
    }
}
