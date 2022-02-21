using System.Net;
using BRentals.Api.Core.Entities;
using BRentals.Api.Infrastructure.Models;
using BRentals.DTOs;
using Convey.CQRS.Queries;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Application.Books.Queries.Handlers;

public class FetchBookHandler : IQueryHandler<FetchBook, BookDto?>
{
    private readonly IRepository<BookScanLookup> _scanLookupRepository;
    private readonly IRepository<Book> _bookRepository;

    public FetchBookHandler(
        IRepository<BookScanLookup> scanLookupRepository,
        IRepository<Book> bookRepository)
    {
        _scanLookupRepository = scanLookupRepository;
        _bookRepository = bookRepository;
    }

    public async Task<BookDto?> HandleAsync(
        FetchBook query,
        CancellationToken cancellationToken)
    {
        var (isbn, category) = query;

        if (category is null)
        {
            try
            {
                var lookup = await _scanLookupRepository.GetAsync(
                    isbn,
                    cancellationToken: cancellationToken);

                category = lookup.Category;
            }
            catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        Book? book;

        try
        {
            book = await _bookRepository.GetAsync(
                isbn,
                category,
                cancellationToken);
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }

        return new BookDto(
            book.Id,
            book.Title,
            book.Category,
            book.Authors,
            book.Published.ToLongDateString());
    }
}