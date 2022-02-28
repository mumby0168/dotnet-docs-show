# dotnet-docs-show - BRentals
The book rental store demo for my demo on the dotnet docs show.

![NET Docs Show](https://user-images.githubusercontent.com/23740684/155897713-6d101d6c-2ac7-40d8-958a-93568ff23b30.jpg)

## Run locally

To run the project locally all you need is a cosmos connection string, this can either from the emulator or a real service. The demo uses the serverless cosmos DB plan as in the cases of demo's it works out a lot cheaper.

Simply navigate to `BRentals.Api` project in a terminal of your choice and run the following command.

```shell
dotnet user-secrets set RepositoryOptions:CosmosConnectionString "<your-conn-string>"
```

Alternatively, you can add this to the `appsettings.Development.json` file in the `BRentals.Api` project.

# API

The API allows some simple tasks on a book.

* [x] Create a book category.
* [x] List categories
* [x] Create a book (partitioned by book category, lookup record for scanning the books barcode).
* [x] Read books in a category. 
* [x] Rent a book, takes the book's barcode & customer's username (e-tags).
* [x] Return a book takes the book's barcode & customer's username (e-tags).
* [x] View Category's.
* [x] View books in a category (load more).
* [x] View a book and see all it's rentals over time (load more `book-rentals`).
* [x] Query rentals by a customer (use change feed to dupe data) (`customer-rentals`).

# Hub/UI (Blazor Server App)

The UI allows the API to be driven in a more interactive manor. The basic functionality is listed below.

- [x] List all categories
- [x] List books in a category
- [x] View a book
- [x] Make a rental on a book
- [x] Search for rentals for a customer
- [x] Return a book for a given rental for a customer
- [x] Search for rentals for a book
