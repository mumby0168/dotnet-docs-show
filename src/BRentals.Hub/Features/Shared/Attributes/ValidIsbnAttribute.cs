using System.ComponentModel.DataAnnotations;
using BRentals.Hub.Features.Shared.Utilities;

namespace BRentals.Hub.Features.Shared.Attributes;

public class ValidIsbnAttribute : RegularExpressionAttribute
{
    public ValidIsbnAttribute() : base(ValidationUtilities.Isbn.Regex) => 
        ErrorMessage= ValidationUtilities.Isbn.Message;
}