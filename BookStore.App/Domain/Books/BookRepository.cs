using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.App.Books
{

    public class BookRepository : IBookRespository
    {
        private readonly BookContext _bookContext;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(BookContext bookContext, ILogger<BookRepository> logger)
        {
            _logger = logger;
            _bookContext = bookContext;
        }

        public async Task<long> AddAsync(Book book)
        {
            await _bookContext.Books.AddAsync(book);
            await _bookContext.SaveChangesAsync();
            return book.Id;
        }

        public async Task<IEnumerable<Book>> FindAllAsync()
        {
            return await _bookContext.Books.ToListAsync();
        }

        public async Task<Book> FindByIdAsync(long id)
        {
            Book book = await _bookContext.Books.FindAsync(id);
            return book;
        }

        public async Task RemoveAsync(Book book)
        {
            _bookContext.Books.Remove(book);
            await _bookContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _logger.LogInformation($"Updating book with id: {book.Id}");
            _bookContext.Entry(book).State = EntityState.Modified;

            try
            {
                await _bookContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookIdExists(book.Id))
                {
                    throw new NotFoundException("Todo item not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        private bool BookIdExists(long id)
        {
            return _bookContext.Books.Any(x => x.Id == id);
        }
    }

}