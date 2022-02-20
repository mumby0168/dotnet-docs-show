using BRentals.Api.Application.Books.Commands;
using BRentals.Api.Application.Books.DTOs;
using BRentals.Api.Application.Books.Queries;
using BRentals.Api.Application.Categories.Commands;
using BRentals.Api.Application.Categories.DTOs;
using BRentals.Api.Application.Categories.Queries;
using BRentals.Api.Core.Entities;
using CleanArchitecture.Exceptions.AspNetCore;
using Convey.CQRS.Queries;

namespace BRentals.Api.Endpoints;

public static class BookEndpoints
{
    private const string Tag = "Books";

    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/books", CQRSHelpers.HandleCommand<CreateBook>())
            .WithTags(Tag)
            .Produces<ErrorDto>(400);

        builder.MapGet("/api/books", GetInCategory)
            .WithTags(Tag)
            .Produces<ErrorDto>(400);

        return builder;
    }

    private static Task<IEnumerable<BookDto>> GetInCategory(IQueryDispatcher dispatcher) =>
        dispatcher.QueryAsync(new FetchBooksInCategory());
}