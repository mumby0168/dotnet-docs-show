using BRentals.Api.Core.Entities;
using BRentals.DTOs;
using Convey.CQRS.Queries;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Specification;

namespace BRentals.Api.Application.Rentals.Queries.Handlers;

public class
    FetchRentalsForUsernameHandler : IQueryHandler<FetchRentalsForUsername, FetchRentalsForUsername.QueryResult>
{
    private readonly IRepository<CustomerRental> _repository;

    public FetchRentalsForUsernameHandler(IRepository<CustomerRental> repository) =>
        _repository = repository;

    public async Task<FetchRentalsForUsername.QueryResult> HandleAsync(
        FetchRentalsForUsername query,
        CancellationToken cancellationToken)
    {
        var (username, results, continuation) = query;

        var specification = new Specification(
            username,
            results,
            continuation);

        var queryResult = await _repository.QueryAsync(
            specification,
            cancellationToken);

        return new FetchRentalsForUsername.QueryResult(
            queryResult.Items.Select(x =>
                new RentalDto(
                    x.Id,
                    x.Book.Isbn,
                    x.Book.Title,
                    x.RentedAt,
                    x.ReturnedAt)),
            queryResult.Continuation);
    }

    private class Specification : ContinuationTokenSpecification<CustomerRental>
    {
        public Specification(
            string username,
            int pageSize,
            string? continuationToken = null)
        {
            Query
                .Where(x => x.PartitionKey == username)
                .OrderByDescending(x => x.RentedAt)
                .PageSize(pageSize)
                .ContinuationToken(continuationToken);
        }
    }
}