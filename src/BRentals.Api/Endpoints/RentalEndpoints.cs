using BRentals.Api.Application.Rentals.Commands;
using CleanArchitecture.Exceptions.AspNetCore;

namespace BRentals.Api.Endpoints;

public static class RentalEndpoints
{
    private const string Tag = "Rentals";

    public static IEndpointRouteBuilder MapRentalEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/rentals/", CQRSHelpers.HandleCommand<RentBook>())
            .WithTags(Tag)
            .Produces<ErrorDto>(400)
            .Produces<ErrorDto>(404);
        
        return builder;
    }
}