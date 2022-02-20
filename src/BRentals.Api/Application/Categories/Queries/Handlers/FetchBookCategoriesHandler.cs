using BRentals.Api.Application.Categories.DTOs;
using BRentals.Api.Core.Entities;
using Convey.CQRS.Queries;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Specification;

namespace BRentals.Api.Application.Categories.Queries.Handlers;

public class FetchBookCategoriesHandler : IQueryHandler<FetchBookCategories, IEnumerable<BookCategoryDto>>
{
    private readonly ILogger<FetchBookCategoriesHandler> _logger;
    private readonly IRepository<BookCategory> _bookCategoryRepository;

    public FetchBookCategoriesHandler(
        ILogger<FetchBookCategoriesHandler> logger,
        IRepository<BookCategory> bookCategoryRepository)
    {
        _logger = logger;
        _bookCategoryRepository = bookCategoryRepository;
    }
    
    public async Task<IEnumerable<BookCategoryDto>> HandleAsync(FetchBookCategories query, CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Fetching book categories");

        var specification = new Specification(query.Results, query.Ordered);

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