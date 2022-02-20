using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Infrastructure.Models;

public class BookScanLookup : FullItem
{
    public string Category { get; }
    public int Copies { get; }

    public BookScanLookup(
        string id, 
        string category,
        int copies)
    {
        Category = category;
        Copies = copies;
        Id = id;
    }
}