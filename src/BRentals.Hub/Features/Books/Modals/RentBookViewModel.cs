using System.ComponentModel.DataAnnotations;

namespace BRentals.Hub.Features.Books.Modals;

public class RentBookViewModel
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; } = null!;
}