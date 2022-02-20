using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Core.Entities;

public class BookCategory : FullItem
{
    public BookCategory(string id)
    {
        PartitionKey = nameof(BookCategory);
        Id = id;
    }
    
    public string PartitionKey { get; set; }

    protected override string GetPartitionKeyValue() =>
        PartitionKey;
}