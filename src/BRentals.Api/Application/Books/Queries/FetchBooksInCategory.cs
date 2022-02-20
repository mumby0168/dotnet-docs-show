using BRentals.Api.Application.Books.DTOs;
using BRentals.Api.Application.Categories.DTOs;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Books.Queries;

public record FetchBooksInCategory() : IQuery<IEnumerable<BookDto>>;