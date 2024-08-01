using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.App.Books
{

    public class BookRepository : IBookRespository
    {
        private readonly BookDbContext _bookDbContext;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(BookDbContext bookDbContext, ILogger<BookRepository> logger)
        {
            _logger = logger;
            _bookDbContext = bookDbContext;
        }

        public async Task<long> AddAsync(Book book)
        {
            await _bookDbContext.Books.AddAsync(book);
            await _bookDbContext.SaveChangesAsync();
            return book.Id;
        }

        public async Task<IEnumerable<Book>> FindAllAsync()
        {
            return await _bookDbContext.Books.ToListAsync();
        }

        public async Task<Book> FindByIdAsync(long id)
        {
            Book book = await _bookDbContext.Books.FindAsync(id);
            return book;
        }

        public async Task RemoveAsync(Book book)
        {
            Book existingBook = _bookDbContext.Books.Where(b => b.Id == book.Id).FirstOrDefault();
            if (existingBook == null)
            {
                throw new NotFoundException("Book not found.");
            }

            _bookDbContext.Books.Remove(existingBook);
            await _bookDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book bookToUpdate)
        {
            _logger.LogInformation($"Updating book with id: {bookToUpdate.Id}");

            Book existingBook = _bookDbContext.Books.Where(b => b.Id == bookToUpdate.Id).FirstOrDefault();

            if (existingBook == null)
            {
                throw new NotFoundException("Book not found.");
            }

            existingBook.Author = bookToUpdate.Author;
            existingBook.Description = bookToUpdate.Description;
            existingBook.Isbn = bookToUpdate.Isbn;
            existingBook.Title = bookToUpdate.Title;
            existingBook.PublishDate = bookToUpdate.PublishDate;

            _bookDbContext.Entry(existingBook).State = EntityState.Modified;

            try
            {
                await _bookDbContext.SaveChangesAsync();
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
            return _bookDbContext.Books.Any(x => x.Id == id);
        }
    }

}