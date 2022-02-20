namespace BRentals.Api.Core.Entities;

public class BookRental : Rental
{
    public BookRental(
        RentedBook book, 
        string customerUsername, 
        DateTime rentedAt, 
        DateTime? returnedAt = null) : 
        base(book.Isbn, book, customerUsername, rentedAt, returnedAt)
    {
    }
}