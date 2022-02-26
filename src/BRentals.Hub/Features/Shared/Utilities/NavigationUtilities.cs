namespace BRentals.Hub.Features.Shared.Utilities;

public static class NavigationUtilities
{
    public static string ViewUserRentals(string username) =>
        $"rentals/customers/{username}";
    
    public static string ViewBook(string category, string isbn) =>
        $"categories/{category}/books/{isbn}";

    public static string ViewRentals(string isbn) =>
        $"/rentals/books/{isbn}";
}