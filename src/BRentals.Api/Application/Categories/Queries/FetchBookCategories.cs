using BRentals.Api.Application.Categories.DTOs;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Categories.Queries;

public record FetchBookCategories(int Results = 20, bool Ordered = true) : IQuery<IEnumerable<BookCategoryDto>>;