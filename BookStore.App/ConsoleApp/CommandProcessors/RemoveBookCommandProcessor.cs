using BookStore.App.Books;

namespace BookStore.App.ConsoleApp
{
    /// <summary>
    /// Processor for command <c>remove BOOK_ID</c>.
    /// Removes the book with the provided BOOK_ID, or do nothing if no books with the provided BOOK_ID is found.
    /// </summary>
    class RemoveBookCommandProcessor : BaseUserCommandProcessor<RemoveBookCommand>
    {
        public const string COMMAND_KEY = "remove";

        public RemoveBookCommandProcessor(IBookService bookService) : base(bookService, COMMAND_KEY)
        {
        }

        public override void PrintUsage()
        {
            Console.WriteLine($"\t{COMMAND_KEY} BOOK_ID");
            Console.WriteLine($"example:\n\t{COMMAND_KEY} 1");
        }

        protected override RemoveBookCommand ParseCommandBody(string body)
        {
            if (long.TryParse(body, out long id))
            {
                return new RemoveBookCommand()
                {
                    Id = id
                };
            }

            return null;
        }

        protected override async Task DoProcess(RemoveBookCommand command)
        {
            try
            {
                await _bookService.RemoveAcync(command.Id);
                Console.WriteLine($"Book with ID {command.Id} removed!");
            }
            catch (NotFoundException)
            {
                Console.WriteLine("Requested book not found!");
            }
        }
    }
}
