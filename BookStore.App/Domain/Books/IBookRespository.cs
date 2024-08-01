
namespace BookStore.App.Books
{
    public interface IBookRespository
    {
        /// <summary>
        /// Add a book to the repository.
        /// </summary>
        /// <param name="book"></param>
        /// <returns>Id of the saved <c>Book</c> entity.</returns>
        Task<long> AddAsync(Book book);

        /// <summary>
        /// Update an existing book in the repository.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        Task UpdateAsync(Book book);

        /// <summary>
        /// Find a book by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The <c>Book</c> entity if found, otherwise null.</returns>
        Task<Book> FindByIdAsync(long id);

        /// <summary>
        /// Remove a book.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        Task RemoveAsync(Book book);

        /// <summary>
        /// Retrives all books in the repository.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Book>> FindAllAsync();
    }
}