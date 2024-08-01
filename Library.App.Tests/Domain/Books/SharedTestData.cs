
namespace Library.App.Books
{
    static class SharedTestData
    {
        public static List<Book> GetTestBooks()
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

        public static Book GetTestBookWithoutId() => new()
        {
            Title = "Book",
            Author = "John Doe",
            Description = "book info",
            PublishDate = new(),
            Isbn = GoodIsbn(),
        };

        public static string GoodIsbn() => "0-061-96436-0";
        public static string AnotherGoodIsbn() => "978-3-16-148410-0";
        public static string BadIsbn() => "NOT-VALID-ONE";
    }
}
