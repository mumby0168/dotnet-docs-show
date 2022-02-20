using BRentals.Api.Core.Extensions;
using CleanArchitecture.Exceptions;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Core.Entities;

public class Book : FullItem
{
    public string Title { get; }
    public string[] Authors { get; }
    public string Category { get; }
    public DateOnly Published { get; }

    public Book(
        string id, 
        string title, 
        string[] authors,
        string category,
        DateOnly published)
    {
        if (!id.IsValidIsbn())
        {
            throw new DomainException<Book>("a valid ISBN must be provided");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException<Book>("a book must have a name");
        }

        if (authors.Length < 1)
        {
            throw new DomainException<Book>("a book must have at least one author");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new DomainException<Book>("a book must be assigned to a category");
        }
        
        Id = id;
        Title = title;
        Authors = authors;
        Category = category;
        Published = published;
        PartitionKey = category;
    }
    
    public string PartitionKey { get; set; }

    protected override string GetPartitionKeyValue() =>
        PartitionKey;
}