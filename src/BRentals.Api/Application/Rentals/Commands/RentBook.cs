using Convey.CQRS.Commands;

namespace BRentals.Api.Application.Rentals.Commands;

public record RentBook(
    string Isbn,
    string CustomerUsername) : ICommand;