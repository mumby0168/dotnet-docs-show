using Blazored.Modal;
using Blazored.Modal.Services;
using BRentals.DTOs;

namespace BRentals.Hub.Features.Books.Modals;

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
}