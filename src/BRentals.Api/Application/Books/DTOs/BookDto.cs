namespace BRentals.Api.Application.Books.DTOs;

public record BookDto(
    string Id,
    string Title,
    string Category,
    string[] Authors,
    string Published);