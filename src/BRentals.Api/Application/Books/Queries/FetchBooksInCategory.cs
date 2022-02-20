using BRentals.Api.Application.Books.DTOs;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Books.Queries;

public record FetchBooksInCategory(
    string Category,
    int PageSize,
    string? ContinuationToken) : IQuery<FetchBooksInCategory.QueryResult>
{
    public record QueryResult(IEnumerable<BookDto> Books, string? ContinuationToken);
}
    