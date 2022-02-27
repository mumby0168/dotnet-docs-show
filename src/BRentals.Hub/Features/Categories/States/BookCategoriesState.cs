using BRentals.DTOs;

namespace BRentals.Hub.Features.Categories.States;

public class BookCategoriesState
{
    private readonly List<string> _categoryNames = new();

    public IReadOnlyList<string> CategoryNames => _categoryNames;

    public void Update(IEnumerable<BookCategoryDto> categories)
    {
        var newNames = categories.Select(x => x.Name).ToList();
        newNames.AddRange(_categoryNames);
        _categoryNames.Clear();
        _categoryNames.AddRange(newNames.Distinct());
    }
}