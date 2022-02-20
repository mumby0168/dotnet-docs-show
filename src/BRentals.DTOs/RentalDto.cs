namespace BRentals.DTOs;

public record RentalDto(
    string Id,
    string Isbn,
    string Title,
    DateTime RentedUtc,
    DateTime? ReturnedUtc);