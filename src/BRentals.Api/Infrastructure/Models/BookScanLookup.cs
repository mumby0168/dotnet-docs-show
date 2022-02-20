using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Infrastructure.Models;

public class BookScanLookup : FullItem
{
    public string Category { get; }

    public BookScanLookup(string id, string category)
    {
        Category = category;
        Id = id;
    }
}