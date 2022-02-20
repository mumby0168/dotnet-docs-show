using BRentals.Api.Core.Aggregates;

namespace BRentals.Api.Core.Repositories;

public interface IBookRentalManagerRepository
{
    ValueTask<BookRentalManager> GetAsync(string isbn, CancellationToken cancellationToken = default);

    ValueTask SaveAsync(BookRentalManager bookRentalManager, CancellationToken cancellationToken = default);
}