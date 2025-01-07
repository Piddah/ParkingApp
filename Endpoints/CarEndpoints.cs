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

            group.MapGet("/{id}", (int id) =>
            {
                var car = Datastore.Users.Find(x => x.Id == id);
                return car is null ? Results.NotFound() : Results.Ok(car);
            }).WithName(getCar);

            group.MapPost("/", (User user, NewCarDto car) =>
            {
                var newCar = new Car(car.Numberplate);

                user.AddCar(newCar);
                return Results.CreatedAtRoute(getCar,new {id = newCar.Id}, newCar );
            });

            return group;
        }
    }
}
