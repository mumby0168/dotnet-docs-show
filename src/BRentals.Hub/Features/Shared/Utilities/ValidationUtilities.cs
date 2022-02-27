namespace BRentals.Hub.Features.Shared.Utilities;

public static class ValidationUtilities
{
    public static class Isbn
    {
        public const string Regex = "[0-9-]{13}";

        public const string Message = "An ISBN can only contain numbers and dashes (-) and be 13 characters";
    }
}