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
}