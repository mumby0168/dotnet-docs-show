using BRentals.DTOs;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Books.Queries;

public record SearchBooks(
    string Term,
    string Category) : IQuery<IEnumerable<BookDto>>;