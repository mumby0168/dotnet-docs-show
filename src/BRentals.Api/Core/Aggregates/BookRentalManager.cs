using BRentals.Api.Core.Entities;
using CleanArchitecture.Exceptions;

namespace BRentals.Api.Core.Aggregates;

public class BookRentalManager
{
    private readonly Book _book;
    private readonly IReadOnlyList<BookRental> _currentRentals;
    private readonly List<BookRental> _updatedRentals = new();

    public BookRentalManager(
        Book book,
        IReadOnlyList<BookRental> currentRentals)
    {
        if (currentRentals.Any(x => x.ReturnedAt is not null))
        {
            throw new DomainException<BookRentalManager>("all rentals must be outstanding");
        }
        
        _book = book;
        _currentRentals = currentRentals;
    }

    public void TryRent(string customerUsername)
    {
        if (string.IsNullOrWhiteSpace(customerUsername))
        {
            throw new DomainException<BookRentalManager>("a customer username must be provided to rent a book");
        }
        
        if (_currentRentals.Count >= _book.Copies)
        {
            throw new DomainException<BookRentalManager>($"the book {_book.Id} has all it's copies rented out");
        }

        var rental = new BookRental(
            new Rental.RentedBook(_book.Id, _book.Title),
            customerUsername,
            DateTime.UtcNow);
        
        _updatedRentals.Add(rental);
    }

    public void Return(string customerUsername)
    {
        var rental = _currentRentals.FirstOrDefault(x => x.CustomerUsername == customerUsername);

        if (rental is null)
        {
            throw new ResourceNotFoundException<BookRental>(
                $"no book rental found for book {_book.Id} for username {customerUsername}");
        }

        rental.MarkReturned();
        _updatedRentals.Add(rental);
    }

    public IReadOnlyList<BookRental> UpdatedRentals => _updatedRentals;
}