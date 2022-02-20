using BRentals.Api.Core.Entities;
using BRentals.Api.Infrastructure.Models;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.ChangeFeed;

namespace BRentals.Api.Infrastructure.Processors;

public class BookChangeFeedProcessor : IItemChangeFeedProcessor<Book>
{
    private readonly ILogger<BookChangeFeedProcessor> _logger;
    private readonly IRepository<BookScanLookup> _bookScanLookupRepository;

    public BookChangeFeedProcessor(
        ILogger<BookChangeFeedProcessor> logger,
        IRepository<BookScanLookup> bookScanLookupRepository)
    {
        _logger = logger;
        _bookScanLookupRepository = bookScanLookupRepository;
    }

    public async ValueTask HandleAsync(Book book, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating book scan lookup record for book with id {BookID} in category {CategoryID}",
            book.Id,
            book.Category);

        var scanLookup = new BookScanLookup(
            book.Id, 
            book.Category, 
            book.Copies);
        
        await _bookScanLookupRepository.UpdateAsync(scanLookup, cancellationToken);

        _logger.LogInformation(
            "Successfully updated book scan lookup record for book with id {BookID} in category {CategoryID}",
            book.Id,
            book.Category);
    }
}