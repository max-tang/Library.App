
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

            List<Book> books = SharedTestData.GetTestBooks();
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

            Book book = SharedTestData.GetTestBookWithoutId();
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
            CreateBookCommand command = GetCreateBookCommand(SharedTestData.GoodIsbn());

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
            Func<Task> action = () => service.AddAcync(GetCreateBookCommand(SharedTestData.BadIsbn()));

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
            Book book = SharedTestData.GetTestBookWithoutId();

            mockBookRepository.Setup(r => r.AddAsync(It.IsAny<Book>())).Throws(() => new ApplicationException("Some error"));

            BookService service = new(mockBookRepository.Object, mockLogger);

            // Act
            Func<Task> action = () => service.AddAcync(GetCreateBookCommand(SharedTestData.GoodIsbn()));

            // Assert
            await Assert.ThrowsAsync<ApplicationException>(action);
        }

        [Fact]
        public async Task UpdateAsync_Succeeds()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = SharedTestData.GetTestBookWithoutId();
            UpdateBookCommand command = GetUpdateBookCommand(SharedTestData.AnotherGoodIsbn());
            book.Id = command.Id;
            mockBookRepository.Setup(r => r.UpdateAsync(It.IsAny<Book>())).Returns(Task.FromResult(true));
            BookService service = new(mockBookRepository.Object, mockLogger);

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
            Book book = SharedTestData.GetTestBookWithoutId();

            mockBookRepository.Setup(r => r.UpdateAsync(It.IsAny<Book>())).Throws(() => new NotFoundException("Requested book not found"));

            BookService service = new(mockBookRepository.Object, mockLogger);

            UpdateBookCommand command = GetUpdateBookCommand(SharedTestData.AnotherGoodIsbn());

            // Act
            Func<Task> action = () => service.UpdateAcync(command);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
            mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Fails_Bad_Isbn()
        {
            // Arrange
            var mockBookRepository = new Mock<IBookRespository>();
            var mockLogger = Mock.Of<ILogger<BookService>>();
            Book book = SharedTestData.GetTestBookWithoutId();
            book.Id = 1;

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(book));

            BookService service = new(mockBookRepository.Object, mockLogger);

            UpdateBookCommand command = GetUpdateBookCommand(SharedTestData.BadIsbn());

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
            Book book = SharedTestData.GetTestBookWithoutId();

            mockBookRepository.Setup(r => r.FindByIdAsync(It.IsAny<long>())).Returns(Task.FromResult(book));
            mockBookRepository.Setup(r => r.UpdateAsync(It.IsAny<Book>())).Throws(() => new ApplicationException());

            BookService service = new(mockBookRepository.Object, mockLogger);

            UpdateBookCommand command = GetUpdateBookCommand(SharedTestData.GoodIsbn());

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
            Book book = SharedTestData.GetTestBookWithoutId();

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
            Book book = SharedTestData.GetTestBookWithoutId();

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
    }
}