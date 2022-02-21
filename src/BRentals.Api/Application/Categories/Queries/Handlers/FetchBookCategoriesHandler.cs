using BRentals.Api.Core.Entities;
using BRentals.DTOs;
using Convey.CQRS.Queries;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.Specification;

namespace BRentals.Api.Application.Categories.Queries.Handlers;

public class FetchBookCategoriesHandler : IQueryHandler<FetchBookCategories, IEnumerable<BookCategoryDto>>
{
    private readonly IRepository<BookCategory> _bookCategoryRepository;
    private readonly IRepository<Book> _bookRepository;

    public FetchBookCategoriesHandler(
        IRepository<BookCategory> bookCategoryRepository,
        IRepository<Book> bookRepository)
    {
        _bookCategoryRepository = bookCategoryRepository;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookCategoryDto>> HandleAsync(FetchBookCategories query,
        CancellationToken cancellationToken)
    {
        var (results, ordered) = query;

        var specification = new Specification(results, ordered);

        var result = await _bookCategoryRepository.QueryAsync(specification, cancellationToken);

        var categories = new List<BookCategoryDto>();

        foreach (var category in result.Items)
        {
            var count =
                await _bookRepository.CountAsync(
                    x => x.PartitionKey == category.Id,
                    cancellationToken);

            categories.Add(new BookCategoryDto(category.Id, count));
        }

        return categories;
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