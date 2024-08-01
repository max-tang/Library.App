using BookStore.App.Books;

namespace BookStore.App.ConsoleApp
{
    /// <summary>
    /// Command processor for command <c>list</c>, prints out all <c>Books</c>.
    /// 
    /// </summary>
    class ListBooksCommandProcessor : BaseUserCommandProcessor<FindBooksQuery>
    {
        private static readonly string LINE_SEPERATOR = new string('-', 72);
        private static readonly string HEADER_MARK = new string('=', 72);

        public const string COMMAND_KEY = "list";
        public ListBooksCommandProcessor(IBookService bookService) : base(bookService, COMMAND_KEY)
        {
        }

        public override void PrintUsage()
        {
            Console.WriteLine($"\t{COMMAND_KEY}");
        }

        protected override FindBooksQuery ParseCommandBody(string body)
        {
            return new FindBooksQuery();
        }

        protected override async Task DoProcess(FindBooksQuery query)
        {
            var books = (await _bookService.FindAsync(new FindBooksQuery())).ToList();
            if (books.Count == 0)
            {
                Console.WriteLine("No books found!");
                return;
            }

            Console.WriteLine(HEADER_MARK);
            Console.WriteLine("{0, 6}| {1, 40}| {2, 20}", "ID", "Title", "Author");

            foreach (Book book in books)
            {
                Console.WriteLine(LINE_SEPERATOR);
                Console.WriteLine("{0, 6}| {1, 40}| {2, 20}", book.Id, book.Title, book.Author);
            }
        }
    }
}