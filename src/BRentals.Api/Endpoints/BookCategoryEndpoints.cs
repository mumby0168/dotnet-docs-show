using BRentals.Api.Application.Categories.Commands;
using BRentals.Api.Application.Categories.Queries;
using BRentals.DTOs;
using CleanArchitecture.Exceptions.AspNetCore;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BRentals.Api.Endpoints;

public static class BookCategoryEndpoints
{
    private const string Tag = "Category";
    public static IEndpointRouteBuilder MapBookCategoryEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/categories", CQRSHelpers.HandleCommand<CreateBookCategory>())
            .WithTags(Tag)
            .Produces<ErrorDto>(400);
        
        builder.MapGet("/api/categories", GetAll)
            .WithTags(Tag)
            .Produces<ErrorDto>(400);

        return builder;
    }

    private static Task<IEnumerable<BookCategoryDto>> GetAll(
        [FromQuery] int? pageSize,
        [FromQuery] bool? ordered,
        IQueryDispatcher dispatcher) =>
        dispatcher.QueryAsync(new FetchBookCategories(pageSize ?? 20, ordered ?? true));
}