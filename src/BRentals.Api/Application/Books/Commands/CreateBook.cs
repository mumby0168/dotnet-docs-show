using Convey.CQRS.Commands;

namespace BRentals.Api.Application.Books.Commands;

public record CreateBook(
    string Isbn, 
    string Title, 
    string Category,
    string[] Authors,
    DateTime Published,
    int Copies) : ICommand;