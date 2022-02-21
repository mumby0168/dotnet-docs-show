namespace BRentals.Hub.Features.Shared.Clients;

public record PagedApiResult<T>(
    bool IsSuccessful, 
    List<T> Results, 
    string? Continuation = null);