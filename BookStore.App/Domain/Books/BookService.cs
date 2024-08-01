
using Microsoft.Extensions.Logging;

namespace BookStore.App.Books
{
    public class BookService : IBookService
    {
        ILogger<BookService> _logger;
        IBookRespository _bookRespository;

        public BookService(IBookRespository bookRespository, ILogger<BookService> logger)
        {
            _logger = logger;
            _bookRespository = bookRespository;
        }

        public async Task<Book> AddAcync(CreateBookCommand createBookCommand)
        {
            if (!IsbnValidator.IsValid(createBookCommand.Isbn))
            {
                throw new BadArgumentException("Invalid ISBN number");
            }

            Book book = new()
            {
                Isbn = createBookCommand.Isbn,
                Title = createBookCommand.Title,
                Author = createBookCommand.Author,
                Description = createBookCommand.Description,
                PublishDate = createBookCommand.PublishDate,
            };
            long id = await _bookRespository.AddAsync(book);

            book.Id = id;
            return book;
        }

        public async Task<IEnumerable<Book>> FindAsync(FindBooksQuery findBooksQuery)
        {
            return await _bookRespository.FindAllAsync();
        }

        public async Task<Book> FindByIdAsync(long bookId)
        {
            Book book = await _bookRespository.FindByIdAsync(bookId);
            return book;
        }

        public async Task RemoveAcync(long bookId)
        {
            Book book = await _bookRespository.FindByIdAsync(bookId);
            if (book == null)
            {
                throw new NotFoundException("Requested book not found!");
            }
            await _bookRespository.RemoveAsync(book);
        }

        public async Task<Book> UpdateAcync(UpdateBookCommand updateBookCommand)
        {
            if (!IsbnValidator.IsValid(updateBookCommand.Isbn))
            {
                throw new BadArgumentException("Invalid ISBN number");
            }

            Book book = new Book()
            {
                Id = updateBookCommand.Id,
                Author = updateBookCommand.Author,
                Title = updateBookCommand.Title,
                Description = updateBookCommand.Description,
                Isbn = updateBookCommand.Isbn,
                PublishDate = updateBookCommand.PublishDate,
            };

            await _bookRespository.UpdateAsync(book);

            return book;
        }
    }
}
