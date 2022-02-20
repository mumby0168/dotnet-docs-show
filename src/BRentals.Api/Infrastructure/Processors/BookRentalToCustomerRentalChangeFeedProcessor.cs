using BRentals.Api.Core.Entities;
using Microsoft.Azure.CosmosRepository;
using Microsoft.Azure.CosmosRepository.ChangeFeed;

namespace BRentals.Api.Infrastructure.Processors;

public class BookRentalToCustomerRentalChangeFeedProcessor : IItemChangeFeedProcessor<BookRental>
{
    private readonly IRepository<CustomerRental> _customerRentals;

    public BookRentalToCustomerRentalChangeFeedProcessor(IRepository<CustomerRental> customerRentals) => 
        _customerRentals = customerRentals;

    public async ValueTask HandleAsync(BookRental bookRental, CancellationToken cancellationToken)
    {
        var customerRental = new CustomerRental(
            bookRental.Book,
            bookRental.CustomerUsername,
            bookRental.RentedAt,
            bookRental.ReturnedAt);

        await _customerRentals.UpdateAsync(customerRental, cancellationToken);
    }
}