using Microsoft.Extensions.DependencyInjection;
using BookStore.App.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookStore.App.ConsoleApp;

ServiceProvider serviceProvider = CreateServices();

Application application = new Application(serviceProvider);
RegisterCommandProcessors(application);
application.Run();

static ServiceProvider CreateServices()
{
    return new ServiceCollection()
        .AddLogging(options =>
            {
                options.ClearProviders();
                options.AddDebug();
            }
        )
        .AddDbContext<BookDbContext>(
            options => options.UseInMemoryDatabase("BookStoreDb")
        )
        .AddScoped<IBookRespository, BookRepository>()
        .AddScoped<IBookService, BookService>()
        .AddScoped<CreateBookCommandProcessor>()
        .AddScoped<UpdateBookCommandProcessor>()
        .AddScoped<ListBooksCommandProcessor>()
        .AddScoped<RemoveBookCommandProcessor>()
        .AddScoped<ShowBookCommandProcessor>()
        .BuildServiceProvider();
}

static void RegisterCommandProcessors(Application application)
{
    application.RegisterCommandProcessor<CreateBookCommandProcessor>(CreateBookCommandProcessor.COMMAND_KEY);
    application.RegisterCommandProcessor<ListBooksCommandProcessor>(ListBooksCommandProcessor.COMMAND_KEY);
    application.RegisterCommandProcessor<UpdateBookCommandProcessor>(UpdateBookCommandProcessor.COMMAND_KEY);
    application.RegisterCommandProcessor<RemoveBookCommandProcessor>(RemoveBookCommandProcessor.COMMAND_KEY);
    application.RegisterCommandProcessor<ShowBookCommandProcessor>(ShowBookCommandProcessor.COMMAND_KEY);
}
