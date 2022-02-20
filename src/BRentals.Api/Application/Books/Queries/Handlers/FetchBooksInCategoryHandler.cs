using BRentals.Api.Application.Books.DTOs;
using BRentals.Api.Application.Categories.DTOs;
using BRentals.Api.Application.Categories.Queries;
using BRentals.Api.Application.Categories.Queries.Handlers;
using BRentals.Api.Core.Entities;
using Convey.CQRS.Queries;
using Microsoft.Azure.CosmosRepository.Specification;

namespace BRentals.Api.Application.Books.Queries.Handlers;

public class FetchBooksInCategoryHandler : IQueryHandler<FetchBooksInCategory, IEnumerable<BookDto>>
{
    private readonly ILogger<FetchBooksInCategoryHandler> _logger;

    public FetchBooksInCategoryHandler(ILogger<FetchBooksInCategoryHandler> logger)
    {
        _logger = logger;
    }

    public Task<IEnumerable<BookDto>> HandleAsync(FetchBooksInCategory query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}