using CleanArchitecture.Exceptions;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Core.Entities;

public class BookCategory : FullItem
{
    public BookCategory(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new DomainException<BookCategory>("A book category name must be provided");
        }
        
        PartitionKey = nameof(BookCategory);
        Id = id;
    }
    
    public string PartitionKey { get; set; }

    protected override string GetPartitionKeyValue() =>
        PartitionKey;
}