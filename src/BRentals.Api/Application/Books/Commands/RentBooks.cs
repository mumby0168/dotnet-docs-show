using Convey.CQRS.Commands;

namespace BRentals.Api.Application.Books.Commands;

public record RentBooks(
    string CustomerUsername, 
    List<RentBooks.BookRentalData> Books) : ICommand
{
    public record BookRentalData(string Isbn);
};