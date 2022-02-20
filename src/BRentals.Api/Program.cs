using BRentals.Api.Application.Extensions;
using BRentals.Api.Endpoints;
using BRentals.Api.Infrastructure.Extensions;
using CleanArchitecture.Exceptions.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBRentalsApplicationLayer()
    .AddBRentalsInfrastructureLayer(builder.Environment);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services.AddCleanArchitectureExceptionsHandler(options => options.ApplicationName = "BRentals.Api");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCleanArchitectureExceptionsHandler();

app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

app
    .MapBookCategoryEndpoints()
    .MapBookEndpoints()
    .MapRentalEndpoints();

app.Run();