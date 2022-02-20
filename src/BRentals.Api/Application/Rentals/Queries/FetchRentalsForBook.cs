using BRentals.DTOs;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Rentals.Queries;

public record FetchRentalsForBook(
    string Isbn,
    int Results,
    string? Continuation) : IQuery<FetchRentalsForBook.QueryResult>
{
    public record QueryResult(IEnumerable<RentalDto> Rentals, string? ContinuationToken);
}