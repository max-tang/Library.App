using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Library.App.Books
{
    public class BookRepositoryTests
    {
        [Fact]
        public async Task FindAllAsync_Succeeds()
        {
            // Arrange
            var bookDbContext = CreateTestDbContext();
            var testBooks = SharedTestData.GetTestBooks();
            bookDbContext.Books.AddRange(testBooks.AsEnumerable());
            bookDbContext.SaveChanges();
            var mockLogger = Mock.Of<ILogger<BookRepository>>();

            BookRepository bookRepository = new BookRepository(bookDbContext, mockLogger);

            // Act
            var result = await bookRepository.FindAllAsync();

            // Assert
            Assert.Equal(testBooks, result.ToList());
        }

        [Fact]
        public async Task FindByIdAsync_Succeeds()
        {
            // Arrange
            var bookDbContext = CreateTestDbContext();
            var testBooks = SharedTestData.GetTestBooks();
            bookDbContext.Books.AddRange(testBooks.AsEnumerable());
            bookDbContext.SaveChanges();
            var mockLogger = Mock.Of<ILogger<BookRepository>>();

            BookRepository bookRepository = new BookRepository(bookDbContext, mockLogger);
            long bookId = testBooks[0].Id;

            // Act
            var result = await bookRepository.FindByIdAsync(bookId);

            // Assert
            Assert.Equal(testBooks[0], result);
        }

        [Fact]
        public async Task FindByIdAsync_NotFound()
        {
            // Arrange
            var bookDbContext = CreateTestDbContext();
            var mockLogger = Mock.Of<ILogger<BookRepository>>();

            BookRepository bookRepository = new BookRepository(bookDbContext, mockLogger);
            long bookId = 123523L;

            // Act
            var result = await bookRepository.FindByIdAsync(bookId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_Succeeds()
        {
            // Arrange
            var bookDbContext = CreateTestDbContext();
            var mockLogger = Mock.Of<ILogger<BookRepository>>();

            BookRepository bookRepository = new BookRepository(bookDbContext, mockLogger);
            var book = SharedTestData.GetTestBookWithoutId();

            // Act
            var result = await bookRepository.AddAsync(book);

            // Assert
            var matched = bookDbContext.Books.Where(b => b.Id == result).ToArray();
            Assert.Single(matched);
            Assert.Equal(book, matched[0]);
        }

        [Fact]
        public async Task UpdateAsync_Succeeds()
        {
            // Arrange
            var bookDbContext = CreateTestDbContext();
            var mockLogger = Mock.Of<ILogger<BookRepository>>();

            BookRepository bookRepository = new BookRepository(bookDbContext, mockLogger);
            var book = SharedTestData.GetTestBookWithoutId();
            bookDbContext.Books.Add(book);
            bookDbContext.SaveChanges();

            var bookToBeUpdatedTo = new Book()
            {
                Id = book.Id,
                Author = "Author updated",
                Title = "New book Title",
                Description = "New Description",
                PublishDate = new(),
                Isbn = SharedTestData.AnotherGoodIsbn(),
            };

            // Act
            await bookRepository.UpdateAsync(bookToBeUpdatedTo);

            // Assert
            var matched = bookDbContext.Books.Where(b => b.Id == book.Id).ToArray();
            Assert.Single(matched);
            Assert.Equal(bookToBeUpdatedTo.Author, matched[0].Author);
            Assert.Equal(bookToBeUpdatedTo.Title, matched[0].Title);
            Assert.Equal(bookToBeUpdatedTo.Description, matched[0].Description);
            Assert.Equal(bookToBeUpdatedTo.Isbn, matched[0].Isbn);
            Assert.Equal(bookToBeUpdatedTo.PublishDate, matched[0].PublishDate);
        }

        [Fact]
        public async Task UpdateAsync_Fails_NotFound()
        {
            // Arrange
            var bookDbContext = CreateTestDbContext();
            var mockLogger = Mock.Of<ILogger<BookRepository>>();

            BookRepository bookRepository = new BookRepository(bookDbContext, mockLogger);
            var book = SharedTestData.GetTestBookWithoutId();
            book.Id = 1; // The db is empty, this book should not exist

            // Act
            Func<Task> action = () => bookRepository.UpdateAsync(book);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async Task RemoveAsync_Succeeds()
        {
            // Arrange
            var bookDbContext = CreateTestDbContext();
            var mockLogger = Mock.Of<ILogger<BookRepository>>();

            BookRepository bookRepository = new BookRepository(bookDbContext, mockLogger);
            var book = SharedTestData.GetTestBookWithoutId();
            bookDbContext.Books.Add(book);
            bookDbContext.SaveChanges();

            // Act
            await bookRepository.RemoveAsync(book);

            // Assert
            var booksFindById = bookDbContext.Books.Where(b => b.Id == book.Id).ToArray();
            Assert.Empty(booksFindById);
        }

        [Fact]
        public async Task RemoveAsync_NotFound()
        {
            // Arrange
            var bookDbContext = CreateTestDbContext();
            var mockLogger = Mock.Of<ILogger<BookRepository>>();

            BookRepository bookRepository = new BookRepository(bookDbContext, mockLogger);
            var book = SharedTestData.GetTestBookWithoutId();

            // Act
            Func<Task> action = () => bookRepository.RemoveAsync(book);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        private BookDbContext CreateTestDbContext()
        {
            var options = new DbContextOptionsBuilder<BookDbContext>()
                .UseInMemoryDatabase("TestBookDb")
                .Options;

            var dbContext = new BookDbContext(options);
            dbContext.Database.EnsureDeleted();
            return dbContext;
        }
    }
}
