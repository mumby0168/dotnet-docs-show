using CleanArchitecture.Exceptions;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Core.Entities;

public class Rental : FullItem
{
    public RentedBook Book { get; }
    public string CustomerUsername { get; }
    public DateTime RentedAt { get; }
    public DateTime? ReturnedAt { get; private set; }

    protected Rental(
        string partitionKey,
        RentedBook book,
        string customerUsername,
        DateTime rentedAt,
        DateTime? returnedAt)
    {
        PartitionKey = partitionKey;
        Book = book;
        CustomerUsername = customerUsername;
        RentedAt = rentedAt;
        ReturnedAt = returnedAt;
    }
    
    public string PartitionKey { get; }

    protected override string GetPartitionKeyValue() =>
        PartitionKey;
    
    public record RentedBook(string Isbn, string Title);

    public void MarkReturned()
    {
        if (ReturnedAt is not null)
        {
            throw new DomainException<BookRental>($"The book rental with ID {Id} was already been returned on {ReturnedAt}");
        }
        
        ReturnedAt = DateTime.UtcNow;
    }
}