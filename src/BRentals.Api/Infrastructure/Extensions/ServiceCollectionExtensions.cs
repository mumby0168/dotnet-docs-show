using BRentals.Api.Core.Entities;
using BRentals.Api.Infrastructure.Constants;
using BRentals.Api.Infrastructure.Models;
using Microsoft.Azure.CosmosRepository.AspNetCore.Extensions;

namespace BRentals.Api.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string DatabaseId = "brentals-db";
    public static IServiceCollection AddBRentalsInfrastructureLayer(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        // Configures all the services and IRepository<TItem> instances.
        services.AddCosmosRepository(options =>
        {
            // Tells the library to not assume all items will be in the same container.
            options.ContainerPerItemType = true;
            
            // Sets the database ID - Prefix with machine name in dev.
            options.DatabaseId = environment.IsDevelopment()
                ? $"{Environment.MachineName.ToLower()}|{DatabaseId}"
                : DatabaseId;

            var containerBuilder = options.ContainerBuilder;

            //Configures the container in which BookCategory items will be stored
            containerBuilder.Configure<BookCategory>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.Books)
                    .WithPartitionKey(PartitionKeys.Default)
                    .WithServerlessThroughput());
            
            //Configures the container in which Book items will be stored
            containerBuilder.Configure<Book>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.Books)
                    .WithPartitionKey(PartitionKeys.Default)
                    .WithServerlessThroughput()
                    .WithChangeFeedMonitoring());
            
            //Configures the container in which BookScanLookup items will be stored
            containerBuilder.Configure<BookScanLookup>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.Lookups)
                    .WithPartitionKey(PartitionKeys.Lookups)
                    .WithServerlessThroughput());
        });

        //Registers all IItemChangeFeedProcessor<T>'s
        services.AddCosmosRepositoryItemChangeFeedProcessors(typeof(ServiceCollectionExtensions).Assembly);
        
        // Starts a ASPNET CORE hosted service to listen to the change feed
        services.AddCosmosRepositoryChangeFeedHostedService();

        return services;
    }
}