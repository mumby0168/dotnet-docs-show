namespace BRentals.DTOs;

public record RentalDto(
    string Id,
    string Isbn,
    string Title,
    string CustomerUsername,
    DateTime RentedUtc,
    DateTime? ReturnedUtc);