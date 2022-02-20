using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Core.Entities;

public class Book : FullItem
{
    public string Id { get; }
    public string Title { get; }
    public string Author { get; }
    public string Category { get; }

    public Book(
        string id, 
        string title, 
        string author,
        string category)
    {
        Id = id;
        Title = title;
        Author = author;
        Category = category;
        PartitionKey = category;
    }
    
    public string PartitionKey { get; set; }

    protected override string GetPartitionKeyValue() =>
        PartitionKey;
}