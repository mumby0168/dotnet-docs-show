using System.Net;
using System.Text;
using BRentals.DTOs;
using BRentals.Hub.Features.Shared.Extensions;

namespace BRentals.Hub.Features.Shared.Clients;

public class BRentalsApiClient : IBRentalsApiClient
{
    private readonly HttpClient _client;
    private const string ContinuationHeader = "X-Continuation";

    public BRentalsApiClient(HttpClient client) =>
        _client = client;

    public async ValueTask<PagedApiResult<BookCategoryDto>> GetBookCategoriesAsync(
        string? continuation = null,
        int pageSize = 25)
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

    public async ValueTask<PagedApiResult<BookDto>> GetBooksInCategoryAsync(
        string category,
        string? continuation = null, 
        int pageSize = 25)
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

    public async ValueTask<BookDto?> GetBook(
        string isbn, 
        string? category)
    {
        var url = new StringBuilder()
            .Append($"books/{isbn}");

        if (category is not null)
        {
            url.Append($"?category={category}");
        }

        var response = await _client.GetAsync(url.ToString());

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BookDto>();
        }

        return null;
    }

    public async ValueTask<string?> RentBook(
        string isbn, 
        string username)
    {
        var responseMessage = await _client.PostAsJsonAsync("rentals", new RentBookRequest(isbn, username));

        if (responseMessage.IsSuccessStatusCode)
        {
            return null;
        }

        if (responseMessage.StatusCode is HttpStatusCode.BadRequest)
        {
            var failureResponse = await responseMessage.Content.ReadFromJsonAsync<FailureResponse>();
            if (failureResponse is not null && failureResponse.Errors.Any())
            {
                return failureResponse.Errors.First().Message;
            }
        }

        return $"Something went wrong reserving the book {isbn}";
    }

    public async ValueTask<PagedApiResult<RentalDto>> GetBookRentals(
        string isbn, 
        int pageSize = 20, 
        string? continuation = null)
    {
        _client.TrySetHeader(ContinuationHeader, continuation);

        var response = await _client.GetAsync($"rentals/books?isbn={isbn}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            return new PagedApiResult<RentalDto>(
                true,
                await response.Content.ReadFromJsonAsync<List<RentalDto>>() ?? new List<RentalDto>(),
                response.TryGetHeader(ContinuationHeader));
        }

        return new PagedApiResult<RentalDto>(false, new List<RentalDto>());
    }

    public async ValueTask<PagedApiResult<RentalDto>> GetCustomerRentals(
        string username, 
        int pageSize = 20, 
        string? continuation = null)
    {
        _client.TrySetHeader(ContinuationHeader, continuation);

        var response = await _client.GetAsync($"rentals/books?username={username}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            return new PagedApiResult<RentalDto>(
                true,
                await response.Content.ReadFromJsonAsync<List<RentalDto>>() ?? new List<RentalDto>(),
                response.TryGetHeader(ContinuationHeader));
        }

        return new PagedApiResult<RentalDto>(false, new List<RentalDto>());
    }

    private record RentBookRequest(string Isbn, string Username);

    private record FailureResponse(
        string ApplicationName, 
        List<FailureResponse.Error> Errors)
    {
        public record Error(string Message, string Code, string? ResourceName);
    }
}