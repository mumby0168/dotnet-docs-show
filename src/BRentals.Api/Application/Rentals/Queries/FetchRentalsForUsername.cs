using BRentals.DTOs;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Rentals.Queries;

public record FetchRentalsForUsername(
    string Username,
    int Results,
    string? Continuation) : IQuery<FetchRentalsForUsername.QueryResult>
{
    public record QueryResult(IEnumerable<RentalDto> Books, string? ContinuationToken);
}