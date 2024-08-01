using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;

namespace BookStore.App.Books
{
    public class BookContext : DbContext
    {
        public BookContext()
        {
        }

        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
