
namespace BookStore.App.Books
{

    /// <summary>
    /// Provides the functionality to Create, Update, Read and Delete (CURD) <c>Book</c> models.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Add a book with provided information carried in the command.
        /// </summary>
        /// <param name="createBookCommand"></param>
        /// <returns>The <c>Book</c> entity if created successfully.</returns>
        /// <exception cref="FluentValidation.ValidationException">If input is invalid</exception>
        Task<Book> AddAcync(CreateBookCommand createBookCommand);

        /// <summary>
        /// Update a book with provided information.
        /// </summary>
        /// <param name="updateBookCommand"></param>
        /// <returns>The updated <c>Book</c> entity.</returns>
        /// <exception cref="FluentValidation.ValidationException">If input is invalid</exception>
        /// <exception cref="NotFoundException">If book not found with provided Id.</exception>
        Task<Book> UpdateAcync(UpdateBookCommand updateBookCommand);

        /// <summary>
        /// Find the book with provided <c>bookId</c>.
        /// </summary>
        /// <param name="bookId">Book ID</param>
        /// <returns>The <c>Book</c> entity if found, otherwise null.</returns>
        Task<Book> FindByIdAsync(long bookId);

        /// <summary>
        /// Remove the <c>Book</c> entity with provided bookId.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">If book with bookId not found.</exception>
        Task RemoveAcync(long bookId);

        /// <summary>
        /// Retrieves all books matching the provided criteria.
        /// </summary>
        /// <param name="findBooksQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<Book>> FindAsync(FindBooksQuery findBooksQuery);
    }
}
