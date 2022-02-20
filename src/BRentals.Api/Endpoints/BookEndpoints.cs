using BRentals.Api.Application.Books.Commands;
using BRentals.Api.Application.Books.Queries;
using BRentals.DTOs;
using CleanArchitecture.Exceptions.AspNetCore;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;

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

    private static async Task<IEnumerable<BookDto>> GetInCategory(
        IQueryDispatcher dispatcher,
        HttpContext httpContext,
        [FromQuery] string category,
        [FromQuery] int pageSize = 25,
        [FromHeader(Name = ApiConstants.ContinuationHeader)]
        string? continuationToken = null)
    {
        var query = new FetchBooksInCategory(
            category,
            pageSize,
            continuationToken);

        var (bookDtos, token) = await dispatcher.QueryAsync(query);

        if (token is not null)
        {
            httpContext.Response.Headers.Add(ApiConstants.ContinuationHeader, token);
        }

        return bookDtos;
    }
}