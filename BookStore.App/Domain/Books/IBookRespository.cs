
namespace BookStore.App.Books
{
    public interface IBookRespository
    {
        Task<long> AddAsync(Book book);

        Task UpdateAsync(Book book);

        Task<Book> FindByIdAsync(long id);

        Task RemoveAsync(Book book);

        Task<IEnumerable<Book>> FindAllAsync();
    }
}