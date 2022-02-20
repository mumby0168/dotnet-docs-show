using BRentals.Api.Core.Repositories;
using Convey.CQRS.Commands;

namespace BRentals.Api.Application.Rentals.Commands.Handlers;

public class ReturnBookHandler : ICommandHandler<ReturnBook>
{
    private readonly IBookRentalManagerRepository _repository;

    public ReturnBookHandler(IBookRentalManagerRepository repository) => 
        _repository = repository;

    public async Task HandleAsync(ReturnBook command, CancellationToken cancellationToken = new CancellationToken())
    {
        var (isbn, customerUsername) = command;

        var rentalManager =
            await _repository.GetAsync(
                isbn,
                cancellationToken);

        rentalManager.Return(customerUsername);

        await _repository.SaveAsync(
            rentalManager,
            cancellationToken);
    }
}