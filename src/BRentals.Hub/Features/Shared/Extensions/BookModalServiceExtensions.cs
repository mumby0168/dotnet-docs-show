using Blazored.Modal;
using Blazored.Modal.Services;
using BRentals.DTOs;
using BRentals.Hub.Features.Books.Modals;
using BRentals.Hub.Features.Rentals.Modals;

namespace BRentals.Hub.Features.Shared.Extensions;

public static class BookModalServiceExtensions
{
    public static IModalReference ShowRentBookModal(this IModalService modalService, BookDto bookDto)
    {
        var modalParameters = new ModalParameters();
        modalParameters.Add(nameof(RentBookModal.Book), bookDto);
        
        var options = new ModalOptions {UseCustomLayout = true};
        
        return modalService.Show<RentBookModal>(
            $"Rent {bookDto.Title}", 
            modalParameters, 
            options);
    }
    
    public static IModalReference ShowReturnBookModal(
        this IModalService modalService, 
        RentalDto rental)
    {
        var modalParameters = new ModalParameters();
        modalParameters.Add(nameof(ReturnBookModal.BookTitle), rental.Title);
        modalParameters.Add(nameof(ReturnBookModal.Username), rental.CustomerUsername);
        modalParameters.Add(nameof(ReturnBookModal.Isbn), rental.Isbn);

        var options = new ModalOptions {UseCustomLayout = true};
        
        return modalService.Show<ReturnBookModal>(
            $"Return {rental.Title}", 
            modalParameters, 
            options);
    }
}