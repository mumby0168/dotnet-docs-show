using BRentals.Api.Core.Entities;
using BRentals.DTOs;
using Convey.CQRS.Queries;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Specification;

namespace BRentals.Api.Application.Books.Queries.Handlers;

public class SearchBooksHandler : IQueryHandler<SearchBooks, IEnumerable<BookDto>>
{
    private readonly IRepository<Book> _repository;

    public SearchBooksHandler(IRepository<Book> repository) =>
        _repository = repository;

    public async Task<IEnumerable<BookDto>> HandleAsync(SearchBooks query, CancellationToken cancellationToken)
    {
        var results = await _repository.QueryAsync(
            new Specification(query.Term, query.Category),
            cancellationToken);

        return results.Items.Select(x =>
            new BookDto(
                x.Id,
                x.Title,
                x.Category,
                x.Authors,
                x.Published.ToDateTime(TimeOnly.MaxValue).ToString("d")));
    }

    private class Specification : DefaultSpecification<Book>
    {
        public Specification(string term, string category) =>
            Query.Where(x => x.PartitionKey == category && x.Title.ToLower().Contains(term.ToLower()));
    }
}