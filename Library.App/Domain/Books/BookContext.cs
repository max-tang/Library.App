using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;

namespace Library.App.Books
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
