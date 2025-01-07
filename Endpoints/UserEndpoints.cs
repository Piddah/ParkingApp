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
                var user = users.SingleOrDefault(user => user.Id == id);

                return user is null ? Results.NotFound() : 
                                      Results.Ok(user);

            }).WithName(getUserEndpoint);

          

            group.MapPost("/", (NewUserDto user) =>
            {
                User newUser = new User(
                    user.Firstname,
                    user.Lastname,
                    user.Email,
                    user.Password,
                    user.Cars.Select(numberPlate => new Car(numberPlate)).ToList()
                    );

                users.Add(newUser);
                return Results.CreatedAtRoute(getUserEndpoint, new {id = newUser.Id}, newUser);
            });

            return group;
        }
    }
}
