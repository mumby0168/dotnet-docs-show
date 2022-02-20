using BRentals.Api.Core.Entities;
using BRentals.Api.Core.Repositories;
using BRentals.Api.Infrastructure.Constants;
using BRentals.Api.Infrastructure.Models;
using BRentals.Api.Infrastructure.Repositories;
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
            
            // Configures all the entities and there containers.
            // Look out for the .WithChangeFeedMonitoring()
            // This tells the library to process changes from that container.
            
            containerBuilder.Configure<BookCategory>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.Books)
                    .WithPartitionKey(PartitionKeys.Default)
                    .WithServerlessThroughput());
            
            containerBuilder.Configure<Book>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.Books)
                    .WithPartitionKey(PartitionKeys.Default)
                    .WithServerlessThroughput()
                    .WithChangeFeedMonitoring());
            
            containerBuilder.Configure<BookScanLookup>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.Lookups)
                    .WithPartitionKey(PartitionKeys.Lookups)
                    .WithServerlessThroughput());
            
            containerBuilder.Configure<BookRental>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.BookRentals)
                    .WithPartitionKey(PartitionKeys.Default)
                    .WithServerlessThroughput()
                    .WithChangeFeedMonitoring());
            
            containerBuilder.Configure<CustomerRental>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.CustomerRentals)
                    .WithPartitionKey(PartitionKeys.Default)
                    .WithServerlessThroughput());
        });

        //Registers all IItemChangeFeedProcessor<T>'s
        services.AddCosmosRepositoryItemChangeFeedProcessors(typeof(ServiceCollectionExtensions).Assembly);
        
        // Starts a ASPNET CORE hosted service to listen to the change feed
        services.AddCosmosRepositoryChangeFeedHostedService();

        //Adds the book rental manager repository which combines multiple IRepository<T>'s
        services.AddSingleton<IBookRentalManagerRepository, BookRentalManagerRepository>();

        return services;
    }
}