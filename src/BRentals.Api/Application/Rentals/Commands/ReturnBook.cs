using Convey.CQRS.Commands;

namespace BRentals.Api.Application.Rentals.Commands;

public record ReturnBook(
    string Isbn,
    string CustomerUsername) : ICommand;