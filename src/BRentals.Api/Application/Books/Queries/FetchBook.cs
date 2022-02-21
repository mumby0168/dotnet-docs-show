using BRentals.DTOs;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Books.Queries;

public record FetchBook(
    string Isbn, 
    string? Category) : IQuery<BookDto?>;