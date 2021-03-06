using Blazored.Modal;
using Blazored.Toast;
using BRentals.Hub.Features.Categories.States;
using BRentals.Hub.Features.Shared.Clients;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<IBRentalsApiClient, BRentalsApiClient>(client =>
    client.BaseAddress = builder.Configuration.GetValue<Uri>("ApiBaseUrl"));

builder.Services.AddSingleton<BookCategoriesState>();

builder.Services
    .AddBlazoredModal()
    .AddBlazoredToast();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();