using Convey.CQRS.Commands;

namespace BRentals.Api.Application.Books.Commands.Handlers;

public class RentBooksHandler : ICommandHandler<RentBooks>
{
    public Task HandleAsync(RentBooks command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}