using Convey.CQRS.Commands;

namespace BRentals.Api.Application.Categories.Commands;

public record CreateBookCategory(string Name) : ICommand;