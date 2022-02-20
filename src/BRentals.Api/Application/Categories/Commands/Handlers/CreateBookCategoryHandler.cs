using BRentals.Api.Core.Entities;
using Convey.CQRS.Commands;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Application.Categories.Commands.Handlers;

public class CreateBookCategoryHandler : ICommandHandler<CreateBookCategory>
{
    private readonly ILogger<CreateBookCategoryHandler> _logger;
    private readonly IRepository<BookCategory> _bookCategoryRepository;

    public CreateBookCategoryHandler(
        ILogger<CreateBookCategoryHandler> logger,
        IRepository<BookCategory> bookCategoryRepository)
    {
        _logger = logger;
        _bookCategoryRepository = bookCategoryRepository;
    }
    
    public async Task HandleAsync(CreateBookCategory command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling create book category {BookCategory}", command.Name);

        var category = new BookCategory(command.Name);
        await _bookCategoryRepository.CreateAsync(category, cancellationToken);
        
        _logger.LogInformation("Created book category {BookCategory}", command.Name);
    }
}