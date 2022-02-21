using BRentals.DTOs;
using BRentals.Hub.Features.Shared.Extensions;

namespace BRentals.Hub.Features.Shared.Clients;

public class BRentalsApiClient : IBRentalsApiClient
{
    private readonly HttpClient _client;
    private const string ContinuationHeader = "X-Continuation";

    public BRentalsApiClient(HttpClient client) => 
        _client = client;

    public async ValueTask<PagedApiResult<BookCategoryDto>> GetBookCategoriesAsync(string? continuation = null, int pageSize = 25)
    {
        _client.TrySetHeader(ContinuationHeader, continuation);
        
        var response = await _client.GetAsync($"categories?pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            return new PagedApiResult<BookCategoryDto>(
                true,
                await response.Content.ReadFromJsonAsync<List<BookCategoryDto>>() ?? new List<BookCategoryDto>(), 
                response.TryGetHeader(ContinuationHeader));
        }

        return new PagedApiResult<BookCategoryDto>(false, new List<BookCategoryDto>());
    }

    public async ValueTask<PagedApiResult<BookDto>> GetBooksInCategoryAsync(string category, string? continuation = null, int pageSize = 25)
    {
        _client.TrySetHeader(ContinuationHeader, continuation);
        
        var response = await _client.GetAsync($"books?category={category}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            return new PagedApiResult<BookDto>(
                true,
                await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? new List<BookDto>(), 
                response.TryGetHeader(ContinuationHeader));
        }

        return new PagedApiResult<BookDto>(false, new List<BookDto>());
    }
}