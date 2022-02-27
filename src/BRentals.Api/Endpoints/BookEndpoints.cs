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
            .Produces<ErrorDto>(400)
            .Produces(200);

        builder.MapGet("/api/books", GetInCategory)
            .WithTags(Tag)
            .Produces<ErrorDto>(400)
            .Produces<IEnumerable<BookDto>>();

        builder.MapGet("/api/books/{isbn}", GetBook)
            .WithTags(Tag)
            .Produces(404)
            .Produces<BookDto>();

        builder.MapGet("/api/books/search", SearchBooks)
            .WithTags(Tag)
            .Produces<IEnumerable<BookDto>>()
            .Produces(204)
            .Produces(404);

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

    private static async Task<IResult> SearchBooks(
        IQueryDispatcher dispatcher,
        [FromQuery] string term,
        [FromQuery] string category)
    {
        var result = await dispatcher.QueryAsync(new SearchBooks(term, category));
        return result.Any() ? Results.Ok(result) : Results.NoContent();
    }

    private static async Task<IResult> GetBook(
        IQueryDispatcher dispatcher,
        [FromRoute] string isbn,
        [FromQuery] string? category = null)
    {
        var query = new FetchBook(
            isbn, 
            category);
        
        var book = await dispatcher.QueryAsync(query);

        return book is null ? Results.NotFound() : Results.Ok(book);
    }
}