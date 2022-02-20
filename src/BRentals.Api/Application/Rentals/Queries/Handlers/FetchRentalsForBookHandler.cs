using BRentals.Api.Core.Entities;
using BRentals.DTOs;
using Convey.CQRS.Queries;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Specification;

namespace BRentals.Api.Application.Rentals.Queries.Handlers;

public class FetchRentalsForBookHandler : IQueryHandler<FetchRentalsForBook, FetchRentalsForBook.QueryResult>
{
    private readonly IRepository<BookRental> _repository;

    public FetchRentalsForBookHandler(IRepository<BookRental> repository) =>
        _repository = repository;

    public async Task<FetchRentalsForBook.QueryResult> HandleAsync(
        FetchRentalsForBook query,
        CancellationToken cancellationToken)
    {
        var (isbn, results, continuation) = query;

        var specification = new Specification(
            isbn,
            results,
            continuation);

        var queryResult = await _repository.QueryAsync(
            specification,
            cancellationToken);

        return new FetchRentalsForBook.QueryResult(
            queryResult.Items.Select(x =>
                new RentalDto(
                    x.Id,
                    x.Book.Isbn,
                    x.Book.Title,
                    x.CustomerUsername,
                    x.RentedAt,
                    x.ReturnedAt)),
            queryResult.Continuation);
    }

    private class Specification : ContinuationTokenSpecification<BookRental>
    {
        public Specification(
            string isbn,
            int pageSize,
            string? continuationToken = null)
        {
            Query
                .Where(x => x.PartitionKey == isbn)
                .OrderByDescending(x => x.RentedAt)
                .PageSize(pageSize)
                .ContinuationToken(continuationToken);
        }
    }
}