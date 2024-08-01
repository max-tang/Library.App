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
            Book existingBook = _bookContext.Books.Where(b => b.Id == book.Id).FirstOrDefault();
            if (existingBook == null)
            {
                throw new NotFoundException("Book not found.");
            }

            _bookContext.Books.Remove(existingBook);
            await _bookContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book bookToUpdate)
        {
            _logger.LogInformation($"Updating book with id: {bookToUpdate.Id}");

            Book existingBook = _bookContext.Books.Where(b => b.Id == bookToUpdate.Id).FirstOrDefault();

            if (existingBook == null)
            {
                throw new NotFoundException("Book not found.");
            }

            existingBook.Author = bookToUpdate.Author;
            existingBook.Description = bookToUpdate.Description;
            existingBook.Isbn = bookToUpdate.Isbn;
            existingBook.Title = bookToUpdate.Title;
            existingBook.PublishDate = bookToUpdate.PublishDate;

            _bookContext.Entry(existingBook).State = EntityState.Modified;

            try
            {
                await _bookContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookIdExists(bookToUpdate.Id))
                {
                    throw new NotFoundException("Book not found.");
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