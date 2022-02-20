using BRentals.Api.Core.Entities;
using CleanArchitecture.Exceptions;
using Convey.CQRS.Commands;
using Microsoft.Azure.CosmosRepository;

namespace BRentals.Api.Application.Books.Commands.Handlers;

public class CreateBookHandler : ICommandHandler<CreateBook>
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<BookCategory> _bookCategoryRepository;

    public CreateBookHandler(
        IRepository<Book> bookRepository,
        IRepository<BookCategory> bookCategoryRepository)
    {
        _bookRepository = bookRepository;
        _bookCategoryRepository = bookCategoryRepository;
    }

    public async Task HandleAsync(CreateBook command, CancellationToken cancellationToken)
    {
        var (isbn, title, category, authors, published) = command;

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
    }
}