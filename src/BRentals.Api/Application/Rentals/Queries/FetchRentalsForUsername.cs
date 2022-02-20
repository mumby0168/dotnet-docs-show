namespace BRentals.Api.Application.Rentals.Queries;

public record FetchRentalsForUsername(
    int Results, 
    string? Continuation);