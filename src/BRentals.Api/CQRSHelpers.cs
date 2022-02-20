using Convey.CQRS.Commands;
using Convey.CQRS.Queries;

namespace BRentals.Api;

public static class CQRSHelpers
{
    public static Func<TCommand, ICommandDispatcher, Task> HandleCommand<TCommand>() where TCommand : class, ICommand =>
        (command, dispatcher) => dispatcher.SendAsync(command);
}