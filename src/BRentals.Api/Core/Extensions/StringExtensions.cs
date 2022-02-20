namespace BRentals.Api.Core.Extensions;

public static class StringExtensions
{
    public static bool IsValidIsbn(this string value) =>
        string.IsNullOrWhiteSpace(value) is false && 
        value.Length is 13 &&
        value.All(x => char.IsDigit(x) || x is '-');
}