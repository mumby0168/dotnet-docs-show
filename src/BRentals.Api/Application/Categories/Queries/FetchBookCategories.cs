using BRentals.DTOs;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Categories.Queries;

public record FetchBookCategories(int PageSize = 20, bool Ordered = true) : IQuery<IEnumerable<BookCategoryDto>>;