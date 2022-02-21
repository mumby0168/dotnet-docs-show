namespace BRentals.Hub.Features.Shared.Extensions;

public static class HttpResponseMessageExtensions
{
    public static string? TryGetHeader(
        this HttpResponseMessage response,
        string key) =>
        response.Headers.TryGetValues(key, out var values)
            ? values.FirstOrDefault()
            : null;
}