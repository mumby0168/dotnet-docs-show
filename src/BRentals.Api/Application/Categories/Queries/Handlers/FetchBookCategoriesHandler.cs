using BRentals.Api.Core.Entities;
using BRentals.DTOs;
using Convey.CQRS.Queries;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Specification;

namespace BRentals.Api.Application.Categories.Queries.Handlers;

public class FetchBookCategoriesHandler : IQueryHandler<FetchBookCategories, IEnumerable<BookCategoryDto>>
{
    private readonly IRepository<BookCategory> _bookCategoryRepository;

    public FetchBookCategoriesHandler(IRepository<BookCategory> bookCategoryRepository)
    {
        _bookCategoryRepository = bookCategoryRepository;
    }
    
    public async Task<IEnumerable<BookCategoryDto>> HandleAsync(FetchBookCategories query, CancellationToken cancellationToken)
    {
        var (results, ordered) = query;
        
        var specification = new Specification(results, ordered);

        var result = await _bookCategoryRepository.QueryAsync(specification, cancellationToken);

        return result.Items.Select(x => new BookCategoryDto(x.Id));
    }
    
    private class Specification : DefaultSpecification<BookCategory>
    {
        public Specification(int results, bool ordered)
        {
            Query
                .Where(x => x.PartitionKey == nameof(BookCategory))
                .PageSize(results);

            if (ordered)
            {
                Query.OrderBy(x => x.Id);
            }
        }
    }
}