using System.Text.Json;
using BookStore.App.Books;

namespace BookStore.App.ConsoleApp
{
    class UpdateBookCommandProcessor : BaseUserCommandProcessor<UpdateBookCommand>
    {
        public const string COMMAND_KEY = "update";

        public UpdateBookCommandProcessor(IBookService bookService) : base(bookService, COMMAND_KEY)
        {
        }

        public override void PrintUsage()
        {
            string exampleJson = JsonSerializer.Serialize(new UpdateBookCommand()
            {
                Id = 1,
                Isbn = "0-061-96436-0",
                Title = "Big Bang",
                Author = "John Bell",
                Description = "A brief of the book",
                PublishDate = new DateOnly(2024, 1, 20)
            });
            Console.WriteLine($"\t{COMMAND_KEY} JSON");
            Console.WriteLine($"example:\n\t{COMMAND_KEY} {exampleJson}");
        }

        protected override async Task DoProcess(UpdateBookCommand command)
        {
            await _bookService.UpdateAcync(command);
        }
    }

}