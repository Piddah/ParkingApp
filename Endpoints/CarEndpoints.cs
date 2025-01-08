using ParkingApp.Data;
using ParkingApp.Models;

namespace ParkingApp.Endpoints
{
    public static class CarEndpoints
    {
        public static readonly List<Car> cars = new();
        const string getCar = "Get car";

        public static RouteGroupBuilder MapCarEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("cars").WithParameterValidation();

            group.MapGet("/", () => Datastore.Cars);


            return group;
        }
    }
}
