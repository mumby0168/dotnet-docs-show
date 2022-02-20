using BRentals.Api.Core.Repositories;
using Convey.CQRS.Commands;

namespace BRentals.Api.Application.Rentals.Commands.Handlers;

public class RentBookHandler : ICommandHandler<RentBook>
{
    private readonly IBookRentalManagerRepository _rentalManagerRepository;

    public RentBookHandler(IBookRentalManagerRepository rentalManagerRepository) =>
        _rentalManagerRepository = rentalManagerRepository;

    public async Task HandleAsync(RentBook command, CancellationToken cancellationToken)
    {
        var (isbn, customerUsername) = command;

        var rentalManager =
            await _rentalManagerRepository.GetAsync(
                isbn,
                cancellationToken);

        rentalManager.TryRent(customerUsername);

        await _rentalManagerRepository.SaveAsync(
            rentalManager,
            cancellationToken);
    }
}