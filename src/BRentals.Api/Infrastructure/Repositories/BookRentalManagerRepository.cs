using System.Net;
using BRentals.Api.Core.Aggregates;
using BRentals.Api.Core.Entities;
using BRentals.Api.Core.Repositories;
using BRentals.Api.Infrastructure.Models;
using CleanArchitecture.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Infrastructure.Repositories;

public class BookRentalManagerRepository : IBookRentalManagerRepository
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<BookRental> _bookRentalsRepository;
    private readonly IRepository<BookScanLookup> _bookLookupRepository;

    public BookRentalManagerRepository(
        IRepository<Book> bookRepository,
        IRepository<BookRental> bookRentalsRepository,
        IRepository<BookScanLookup> bookLookupRepository)
    {
        _bookRepository = bookRepository;
        _bookRentalsRepository = bookRentalsRepository;
        _bookLookupRepository = bookLookupRepository;
    }

    public async ValueTask<BookRentalManager> GetAsync(
        string isbn, 
        CancellationToken cancellationToken = default)
    {
        BookScanLookup? lookup;
        try
        {
            lookup = await _bookLookupRepository.GetAsync(isbn, cancellationToken: cancellationToken);
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            throw new ResourceNotFoundException<Book>($"No book found with ISBN {isbn}");
        }

        Book? book;

        try
        {
            book = await _bookRepository.GetAsync(isbn, lookup.Category, cancellationToken);
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            throw new ResourceNotFoundException<Book>($"No book found with ISBN {isbn} in category {lookup.Category}");
        }

        var activeRentals =
            await _bookRentalsRepository.GetAsync(
                x => x.PartitionKey == isbn && x.ReturnedAt == null,
                cancellationToken);

        return new BookRentalManager(book, activeRentals.ToList());
    }

    public async ValueTask SaveAsync(
        BookRentalManager bookRentalManager,
        CancellationToken cancellationToken = default) =>
        await _bookRentalsRepository.UpdateAsync(
            bookRentalManager.UpdatedRentals, 
            cancellationToken);
}