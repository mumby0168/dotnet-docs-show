namespace BRentals.Hub.Features.Shared.Extensions;

public static class HttpClientExtensions
{
    public static void TrySetHeader(
        this HttpClient client, 
        string key, 
        string? value)
    {
        if (value is null)
        {
            return;
        }

        if (client.DefaultRequestHeaders.Contains(key))
        {
            client.DefaultRequestHeaders.Remove(key);
        }
        
        client.DefaultRequestHeaders.Add(key, value);
    }
}