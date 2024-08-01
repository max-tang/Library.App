using System.Text.Json;
using BookStore.App.Books;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore.App.ConsoleApp
{
    class CreateBookCommandProcessor : BaseUserCommandProcessor<CreateBookCommand>
    {
        public const string COMMAND_KEY = "add";

        public CreateBookCommandProcessor(IBookService bookService) : base(bookService, COMMAND_KEY)
        {
        }

        public override void PrintUsage()
        {
            Console.WriteLine($"\t{COMMAND_KEY} JSON");
            string exampleJson = JsonSerializer.Serialize(new CreateBookCommand()
            {
                Isbn = "0-061-96436-0",
                Title = "Big event",
                Author = "John Doe",
                Description = "A brief of the book",
                PublishDate = new DateOnly(2024, 1, 20)
            });
            Console.WriteLine($"example:\n\t{COMMAND_KEY} {exampleJson}");
        }

        protected override async Task DoProcess(CreateBookCommand command)
        {
            await _bookService.AddAcync(command);
        }
    }
}
