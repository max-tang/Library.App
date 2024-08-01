using BookStore.App.Books;

namespace BookStore.App.ConsoleApp
{
    class ShowBookCommandProcessor : BaseUserCommandProcessor<FindByIdQuery>
    {
        public const string COMMAND_KEY = "show";
        public ShowBookCommandProcessor(IBookService bookService) : base(bookService, COMMAND_KEY)
        {
        }

        public override void PrintUsage()
        {
            Console.WriteLine($"\t{COMMAND_KEY} BOOK_ID");
        }

        protected override FindByIdQuery ParseCommandBody(string body)
        {
            if (long.TryParse(body, out long id))
            {
                return new FindByIdQuery()
                {
                    Id = id
                };
            }

            return null;
        }

        protected override async Task DoProcess(FindByIdQuery query)
        {
            Book book = await _bookService.FindByIdAsync(query.Id);

            if (book == null)
            {
                Console.WriteLine("Requested book does not exist!");
            }
            else
            {
                Console.WriteLine("{0,12}: {1}", "Id", book.Id);
                Console.WriteLine("{0,12}: {1}", "Titile", book.Title);
                Console.WriteLine("{0,12}: {1}", "Author", book.Author);
                Console.WriteLine("{0,12}: {1}", "Isbn", book.Isbn);
                Console.WriteLine("{0,12}: {1}", "PublishDate", book.PublishDate);
                Console.WriteLine("{0,12}: {1}", "Description", book.Description);
            }
        }
    }
}