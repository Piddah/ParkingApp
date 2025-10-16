using ParkingApp.Data;

namespace ParkingApp.Models
{
    public class Car
    {
        int NextId = 1;
        public int Id { get; private set; }
        public string Numberplate { get; private set; } = string.Empty;
        public Period? Period { get; private set; }


        public Car(string numberplate)
        {
            Id = NextId++;
            Numberplate = numberplate;
        }

        public void StartPeriod(Period period)
        {
            Period = period;
            Period.StartPeriod();
        }

        public void RemovePeriod()
        {
            Period = null;
        }


    }


}
