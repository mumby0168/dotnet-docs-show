using BRentals.Api.Core.Entities;
using CleanArchitecture.Exceptions;
using Convey.CQRS.Commands;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Application.Books.Commands.Handlers;

public class CreateBookHandler : ICommandHandler<CreateBook>
{
    private readonly ILogger<CreateBookHandler> _logger;
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<BookCategory> _bookCategoryRepository;

    public CreateBookHandler(
        ILogger<CreateBookHandler> logger,
        IRepository<Book> bookRepository,
        IRepository<BookCategory> bookCategoryRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _bookCategoryRepository = bookCategoryRepository;
    }

    public async Task HandleAsync(CreateBook command, CancellationToken cancellationToken)
    {
        var (isbn, title, category, authors, published) = command;
        
        _logger.LogInformation("Handling create book {BookIsbn} in category {BookCategory}", isbn, category);

        if (!await _bookCategoryRepository.ExistsAsync(category, nameof(BookCategory), cancellationToken))
        {
            throw new ResourceNotFoundException<BookCategory>($"a book category with the id {category} was not found");
        }

        var book = new Book(
            isbn,
            title,
            authors,
            category,
            DateOnly.FromDateTime(published));

        await _bookRepository.CreateAsync(book, cancellationToken);

        _logger.LogInformation("Created book {BookIsbn} in category {BookCategory}", isbn, category);
    }
}