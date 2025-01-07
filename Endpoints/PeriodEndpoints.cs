using ParkingApp.Data;
using ParkingApp.Dtos;
using ParkingApp.Models;

namespace ParkingApp.Endpoints
{
    public static class PeriodEndpoints
    {
        public static RouteGroupBuilder MapPeriodEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("periods").WithParameterValidation();

            group.MapPost("/{id}", (int id, EndPeriodDto endPeriod) =>
            {
                var period = Datastore.Periods.SingleOrDefault(x => x.Id == id);
                
                return period is null ? Results.NotFound() : 
                                        Results.Ok(period);
            });

            group.MapGet("/{id}", (int id) =>
            {
                var period = Datastore.Periods.SingleOrDefault(x => x.Id == id);
                
                return period is null ? Results.NotFound() :
                                        Results.Ok(period.GetCurrentPeriod());
            });

            group.MapPost("/", (NewPeriodDto startPeriod) =>
            {
                var newPeriod = new Period(startPeriod.Car);
                Datastore.Periods.Add(newPeriod);
                return Results.Ok(newPeriod);
            });

            return group;
        }

    }
}
