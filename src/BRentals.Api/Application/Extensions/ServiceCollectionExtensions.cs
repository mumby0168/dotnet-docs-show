using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;

namespace BRentals.Api.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBRentalsApplicationLayer(this IServiceCollection services)
    {
        services
            .AddConvey()
            .AddCommandHandlers()
            .AddInMemoryCommandDispatcher()
            .AddQueryHandlers()
            .AddInMemoryQueryDispatcher();
        
        return services;
    }
}