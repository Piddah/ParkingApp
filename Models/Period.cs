using ParkingApp.Data;
using ParkingApp.Endpoints;

namespace ParkingApp.Models
{
    public class Period
    {
        int NextId = 1;
        public int Id { get; private set; }
        public Car Car { get; private set; }
        public double RateDaytime { get; private set; } = 14;
        public double RateRestOfTime { get; private set; } = 6;
        public DateTime StartTime {  get; private set; }
        public DateTime? EndTime { get; private set; }

        public Period(Car car)
        {
            Id = NextId++;
            Car = car;
        }

        public void EndPeriod()
        {
            EndTime = DateTime.Now;
        }

        public void StartPeriod()
        {
            StartTime = DateTime.Now;
        }

        public string GetCurrentPeriod()
        {
            EndTime = DateTime.Now;
            return StartTime.ToString() + " - " + EndTime.ToString();
        }
        
        public int CalculateCost()
        {
            if (EndTime == null)
                EndTime = DateTime.Now;

            if (StartTime > EndTime) 
                throw new ArgumentException("Start time needs to be before end time.");

            double totalCost = 0;
            var current = StartTime;
           
            while (current < EndTime)
            {
                double hourlyRate = (current.Hour >= 8 && current.Hour < 18) ? 
                                     RateDaytime : RateRestOfTime;

                totalCost += hourlyRate/60;
                current = current.AddMinutes(1);
            }
            return (int)Math.Round(totalCost);
        }
    }
}
