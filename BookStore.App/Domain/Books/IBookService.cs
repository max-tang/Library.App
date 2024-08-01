
namespace BookStore.App.Books
{

    public interface IBookService
    {
        Task<Book> AddAcync(CreateBookCommand createBookCommand);

        Task<Book> UpdateAcync(UpdateBookCommand updateBookCommand);

        Task<Book> FindByIdAsync(long bookId);

        Task RemoveAcync(long bookId);

        Task<IEnumerable<Book>> FindAsync(FindBooksQuery findBooksQuery);
    }
}
