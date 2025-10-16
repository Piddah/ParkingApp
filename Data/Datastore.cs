using ParkingApp.Models;

namespace ParkingApp.Data
{
    public static class Datastore
    {
        public static List<User> Users =
        [
            new
            (
                "Peter",
                "Andrén",
                "peter@mail.com",
                "password",
                new List<Car> {new("HST753")}
            ),
            new
            (
                "Liam",
                "Andersson",
                "liam@mail.com",
                "password",
                new List<Car> 
                {
                    new ("GDL971"),
                    new ("DRT800")
                }
            )
        ];

        public static List<Car> Cars =
        [
            new
            (
                "VFB834"
            ),
            new
            (
                "HSJ634"
            )
        ];

        public static List<Period> Periods =
        [
            new
            (
                new Car("JKL876")
            ),
            new
            (
                new Car("WER456")
            )
        ];

        public static List<Account> Accounts =
        [
            

        ];
    }
}
