using BRentals.Api.Core.Entities;

namespace BRentals.Api.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string PartitionKey = "/partitionKey";

    private const string DatabaseId = "brentals-db";

    private static class Containers
    {
        public const string Books = "books";
        public const string Rentals = "rentals";
        public const string CustomerRentals = "customer-rentals";
    }
    
    public static IServiceCollection AddBRentalsInfrastructureLayer(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        services.AddCosmosRepository(options =>
        {
            options.ContainerPerItemType = true;
            options.DatabaseId = environment.IsDevelopment()
                ? $"{Environment.MachineName.ToLower()}|{DatabaseId}"
                : DatabaseId;

            var containerBuilder = options.ContainerBuilder;

            containerBuilder.Configure<BookCategory>(containerOptions => 
                containerOptions
                    .WithContainer(Containers.Books)
                    .WithPartitionKey(PartitionKey)
                    .WithManualThroughput());

        });

        return services;
    }
}