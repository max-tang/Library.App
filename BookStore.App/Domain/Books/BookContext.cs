using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;

namespace BookStore.App.Books
{
    public class BookDbContext : DbContext
    {
        public BookDbContext()
        {
        }

        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
