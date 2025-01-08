using ParkingApp.Data;
using ParkingApp.Models;

namespace ParkingApp.Endpoints
{
    public static class UserEndpoints
    {
        public static readonly List<User> users = new();


        const string getUserEndpoint = "Get User";
        

        public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("users").WithParameterValidation();

            group.MapGet("/", () => Datastore.Users);

            group.MapGet("/{id}/cars", (int id) =>
            {
                var user = Datastore.Users.SingleOrDefault(x => x.Id == id);

                return user is null ? Results.NotFound() : 
                                      Results.Ok(user.Cars);
            });

            group.MapGet("/{id}", (int id) =>
            {
                var user = Datastore.Users.SingleOrDefault(user => user.Id == id);

                return user is null ? Results.NotFound($"User with id:{id} was not found.") : 
                                      Results.Ok(user);

            }).WithName(getUserEndpoint);

            group.MapGet("/{id}/account/cost", (int id) =>
            {
                var user = Datastore.Users.SingleOrDefault(x => x.Id == id);
                if (user is null)
                    return Results.NotFound("User not found.");

                user.Account.CalculateDebt();
                return Results.Ok(user.Account.Debt);
            });

            group.MapGet("/{id}/cars/{numberPlate}/period", (int id, string numberPlate) =>
            {
                var user = Datastore.Users.SingleOrDefault(x => x.Id == id);
                if (user is null)
                    return Results.NotFound("User not found.");

                var car = user.Cars.SingleOrDefault(x => x.Numberplate == numberPlate);
                if (car is null)
                    return Results.NotFound("Car not found.");

                return Results.Ok(car.Period);
            });

            group.MapPost("/{id}/cars/{numberPlate}/period", (int id, NewPeriodDto newPeriod) =>
            {
                var user = Datastore.Users.SingleOrDefault(x => x.Id == id);
                if (user is null)
                    return Results.NotFound("User not found.");
                
                var car = user.Cars.SingleOrDefault(x => x.Numberplate == newPeriod.Car.Numberplate);
                if (car is null)
                    return Results.NotFound("Car not found.");

                var period = new Period(car);
                car.StartPeriod(period);
                return Results.CreatedAtRoute("Get Period", new { id = period.Id }, period);
            });

            group.MapGet("/{id}/cars/{numberPlate}", (int id, string numberPlate) =>
            {
                var user = Datastore.Users.SingleOrDefault(x => x.Id == id);
                if (user is null)
                    return Results.NotFound($"User not found.");

                var car = user.Cars.SingleOrDefault(x => x.Numberplate == numberPlate);
                return car is null ? Results.NotFound("Car not found.") : Results.Ok(car);
            });

        group.MapPost("/", (NewUserDto user) =>
            {
                User newUser = new User(
                    user.Firstname,
                    user.Lastname,
                    user.Email,
                    user.Password,
                    user.Cars.Select(numberPlate => new Car(numberPlate)).ToList()
                    );

                Datastore.Users.Add(newUser);
                return Results.CreatedAtRoute(getUserEndpoint, new {id = newUser.Id}, newUser);
            });

            return group;
        }
    }
}
