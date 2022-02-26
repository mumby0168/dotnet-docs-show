using BRentals.Api.Core.Entities;
using BRentals.DTOs;
using Convey.CQRS.Queries;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Specification;

namespace BRentals.Api.Application.Books.Queries.Handlers;

public class FetchBooksInCategoryHandler : IQueryHandler<FetchBooksInCategory, FetchBooksInCategory.QueryResult>
{
    private readonly IRepository<Book> _repository;

    public FetchBooksInCategoryHandler(IRepository<Book> repository) => 
        _repository = repository;

    public async Task<FetchBooksInCategory.QueryResult> HandleAsync(
        FetchBooksInCategory query,
        CancellationToken cancellationToken)
    {
        var (category, pageSize, continuationToken) = query;

        var specification = new Specification(category, pageSize, continuationToken);

        var result = await _repository.QueryAsync(specification, cancellationToken);

        var bookDtos = result.Items.Select(x =>
            new BookDto(
                x.Id,
                x.Title,
                x.Category,
                x.Authors,
                x.Published.ToDateTime(TimeOnly.MaxValue).ToString("d")));

        return new FetchBooksInCategory.QueryResult(bookDtos, result.Continuation);
    }

    private class Specification : ContinuationTokenSpecification<Book>
    {
        public Specification(
            string category,
            int pageSize,
            string? continuationToken = null)
        {
            Query
                .Where(x => x.PartitionKey == category)
                .PageSize(pageSize)
                .ContinuationToken(continuationToken);
        }
    }
}