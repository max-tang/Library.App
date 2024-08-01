using System.Text.Json;
using BookStore.App.Books;

namespace BookStore.App.ConsoleApp
{
    /// <summary>
    /// Processor for command <c> add BOOK_JSON </c>.
    /// Parses the BOOK_JSON body included in the command and create a new book with provided information.
    /// </summary>
    class CreateBookCommandProcessor : BaseUserCommandProcessor<CreateBookCommand>
    {
        public const string COMMAND_KEY = "add";

        public CreateBookCommandProcessor(IBookService bookService) : base(bookService, COMMAND_KEY)
        {
        }

        public override void PrintUsage()
        {
            Console.WriteLine($"\t{COMMAND_KEY} BOOK_JSON");
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
            Book book = await _bookService.AddAcync(command);
            Console.WriteLine($"Book with id {book.Id} saved.");
        }
    }
}
