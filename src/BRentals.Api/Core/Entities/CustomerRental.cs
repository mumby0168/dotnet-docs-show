namespace BRentals.Api.Core.Entities;

public class CustomerRental : Rental
{
    public CustomerRental(
        RentedBook book, 
        string customerUsername, 
        DateTime rentedAt, 
        DateTime? returnedAt) : 
        base(customerUsername, book, customerUsername, rentedAt, returnedAt)
    {
    }
}