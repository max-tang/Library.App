
namespace BookStore.App.Books
{
    public class BookServiceTests
    {
        [Fact]
        public async Task GetBooks_ReturnAll()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();

            List<Book> books = GetTestBooks();
            mockBookRepository.Setup(r => r.FindAllAsync()).Returns(Task.FromResult(books.AsEnumerable()));
            BookService service = new(mockBookRepository.Object, mockLogger);

            // Act
            var result = await service.FindAsync(new FindBooksQuery());

            // Assert
            Assert.Equal(books.AsEnumerable(), result);
        }

        [Fact]
        public async Task GetById_Succeeds()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();

            Book book = GetTestBook();
            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(book));
            BookService service = new(mockBookRepository.Object, mockLogger);

            // Act
            var result = await service.FindByIdAsync(book.Id);

            // Assert
            Assert.Equal(book, result);
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult<Book>(null));
            BookService service = new(mockBookRepository.Object, mockLogger);

            // Act
            var result = await service.FindByIdAsync(1011);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_Succeeds()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();

            long nextBookId = 100;
            mockBookRepository.Setup(r => r.AddAsync(It.IsAny<Book>())).Returns(Task.FromResult(nextBookId));

            BookService service = new(mockBookRepository.Object, mockLogger);
            CreateBookCommand command = GetCreateBookCommand(GoodIsbn());

            // Act
            var result = await service.AddAcync(command);

            // Assert
            Assert.Equal(nextBookId, result.Id);
            Assert.Equal(command.Author, result.Author);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.Isbn, result.Isbn);
            Assert.Equal(command.PublishDate, result.PublishDate);
        }

        [Fact]
        public async Task AddAsync_Fails_With_Bad_Isbn()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();

            BookService service = new(mockBookRepository.Object, mockLogger);

            // Act
            Func<Task> action = () => service.AddAcync(GetCreateBookCommand(BadIsbn()));

            // Assert
            await Assert.ThrowsAsync<BadArgumentException>(action);
            mockBookRepository.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task AddAsync_Fails()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = GetTestBook();

            mockBookRepository.Setup(r => r.AddAsync(It.IsAny<Book>())).Throws(() => new ApplicationException("Some error"));

            BookService service = new(mockBookRepository.Object, mockLogger);

            // Act
            Func<Task> action = () => service.AddAcync(GetCreateBookCommand(GoodIsbn()));

            // Assert
            await Assert.ThrowsAsync<ApplicationException>(action);
        }

        [Fact]
        public async Task UpdateAsync_Succeeds()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = GetTestBook();

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(book));
            mockBookRepository.Setup(r => r.UpdateAsync(It.IsAny<Book>())).Returns(Task.FromResult(book.Id));

            BookService service = new(mockBookRepository.Object, mockLogger);

            UpdateBookCommand command = GetUpdateBookCommand(AnotherGoodIsbn());

            // Act
            var result = await service.UpdateAcync(command);

            // Assert
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(command.Author, result.Author);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.Isbn, result.Isbn);
            Assert.Equal(command.PublishDate, result.PublishDate);
        }

        [Fact]
        public async Task UpdateAsync_Fails_Not_Found()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = GetTestBook();

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult<Book>(null));

            BookService service = new(mockBookRepository.Object, mockLogger);

            UpdateBookCommand command = GetUpdateBookCommand(AnotherGoodIsbn());

            // Act
            Func<Task> action = () => service.UpdateAcync(command);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
            mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Fails_Bad_Isbn()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = GetTestBook();

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(book));

            BookService service = new(mockBookRepository.Object, mockLogger);

            UpdateBookCommand command = GetUpdateBookCommand(BadIsbn());

            // Act
            Func<Task> action = () => service.UpdateAcync(command);

            // Assert
            await Assert.ThrowsAsync<BadArgumentException>(action);
            mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Fails_Repository_Error()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = GetTestBook();

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(book));
            mockBookRepository.Setup(r => r.UpdateAsync(It.IsAny<Book>())).Throws(() => new ApplicationException());

            BookService service = new(mockBookRepository.Object, mockLogger);

            UpdateBookCommand command = GetUpdateBookCommand(GoodIsbn());

            // Act
            Func<Task> action = () => service.UpdateAcync(command);

            // Assert
            await Assert.ThrowsAsync<ApplicationException>(action);
            mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_Succeeds()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = GetTestBook();

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(book));
            mockBookRepository.Setup(r => r.RemoveAsync(It.IsAny<Book>())).Returns(Task.FromResult(false));

            BookService service = new(mockBookRepository.Object, mockLogger);

            // Act
            await service.RemoveAcync(1);

            // Assert
            mockBookRepository.Verify(r => r.RemoveAsync(It.IsAny<Book>()), Times.Once);
            mockBookRepository.Verify(r => r.FindByIdAsync(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_Fails_NotFound()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = GetTestBook();

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult<Book>(null));

            BookService service = new(mockBookRepository.Object, mockLogger);

            // Act
            Func<Task> action = () => service.RemoveAcync(1);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
            mockBookRepository.Verify(r => r.RemoveAsync(It.IsAny<Book>()), Times.Never);
            mockBookRepository.Verify(r => r.FindByIdAsync(It.IsAny<long>()), Times.Once);
        }

        private CreateBookCommand GetCreateBookCommand(string isbn) => new()
        {
            Title = "Book title",
            Author = "Book Author",
            Description = "Book Descripiton",
            PublishDate = new(2017, 1, 18),
            Isbn = isbn,
        };

        private UpdateBookCommand GetUpdateBookCommand(string isbn) => new()
        {
            Id = 1,
            Title = "new book title",
            Author = "Some one else",
            Description = "Some other description",
            PublishDate = new(),
            Isbn = isbn,
        };

        private List<Book> GetTestBooks()
        {
            return new List<Book> {
                new() {
                    Id = 1,
                    Title = "Book One",
                    Author = "John Doe",
                    Isbn = GoodIsbn(),
                    Description = "",
                    PublishDate = new()
                },
                new() {
                    Id = 2,
                    Title = "Book Two",
                    Author = "John Doe",
                    Isbn = AnotherGoodIsbn(),
                    Description = "",
                    PublishDate = new()
                }
            };
        }

        private Book GetTestBook() => new()
        {
            Id = 3,
            Title = "Book",
            Author = "John Doe",
            Description = "book info",
            PublishDate = new(),
            Isbn = GoodIsbn(),
        };

        private string GoodIsbn() => "0-061-96436-0";
        private string AnotherGoodIsbn() => "978-3-16-148410-0";
        private string BadIsbn() => "NOT-VALID-ONE";
    }
}