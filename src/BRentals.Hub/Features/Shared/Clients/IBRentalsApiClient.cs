using BRentals.DTOs;

namespace BRentals.Hub.Features.Shared.Clients;

public interface IBRentalsApiClient
{
    ValueTask<PagedApiResult<BookCategoryDto>> GetBookCategoriesAsync(
        string? continuation = null,
        int pageSize = 25);
    
    ValueTask<PagedApiResult<BookDto>> GetBooksInCategoryAsync(
        string category,
        string? continuation = null,
        int pageSize = 25);

    ValueTask<BookDto?> GetBook(
        string isbn,
        string? category);

    ValueTask<string?> RentBook(
        string isbn,
        string username);

    ValueTask<PagedApiResult<RentalDto>> GetBookRentals(
        string isbn,
        int pageSize = 20,
        string? continuation = null);
    
    ValueTask<PagedApiResult<RentalDto>> GetCustomerRentals(
        string username,
        int pageSize = 20,
        string? continuation = null);

    ValueTask<string?> ReturnBook(
        string username,
        string isbn);
}