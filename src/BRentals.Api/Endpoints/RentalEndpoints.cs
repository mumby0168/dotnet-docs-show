using BRentals.Api.Application.Rentals.Commands;
using BRentals.Api.Application.Rentals.Queries;
using BRentals.DTOs;
using CleanArchitecture.Exceptions.AspNetCore;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;

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

        builder.MapPut("/api/rentals/return", CQRSHelpers.HandleCommand<ReturnBook>())
            .WithTags(Tag)
            .Produces<ErrorDto>(400)
            .Produces<ErrorDto>(404);
        
        builder.MapGet("/api/rentals", GetInCategory)
            .WithTags(Tag)
            .Produces<ErrorDto>(400)
            .Produces<ErrorDto>(404);

        return builder;
    }

    private static async Task<IEnumerable<RentalDto>> GetInCategory(
        IQueryDispatcher dispatcher,
        HttpContext httpContext,
        [FromQuery] string username,
        [FromQuery] int pageSize = 25,
        [FromHeader(Name = ApiConstants.ContinuationHeader)]
        string? continuationToken = null)
    {
        var query = new FetchRentalsForUsername(
            username,
            pageSize,
            continuationToken);

        var (rentalDtos, token) = await dispatcher.QueryAsync(query);

        if (token is not null)
        {
            httpContext.Response.Headers.Add(ApiConstants.ContinuationHeader, token);
        }

        return rentalDtos;
    }
}