using BRentals.Api.Core.Entities;
using Convey.CQRS.Commands;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Application.Categories.Commands.Handlers;

public class CreateBookCategoryHandler : ICommandHandler<CreateBookCategory>
{
    private readonly IRepository<BookCategory> _bookCategoryRepository;

    public CreateBookCategoryHandler(IRepository<BookCategory> bookCategoryRepository) => 
        _bookCategoryRepository = bookCategoryRepository;

    public async Task HandleAsync(CreateBookCategory command, CancellationToken cancellationToken)
    {
        var category = new BookCategory(command.Name);
        
        await _bookCategoryRepository.CreateAsync(category, cancellationToken);
    }
}