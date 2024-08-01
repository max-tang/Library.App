using System.ComponentModel.DataAnnotations;

namespace Library.App.Books
{
    /// <summary>
    /// Represents a user query to find matching books in the system.
    /// </summary>
    public record FindBooksQuery()
    {
        public string SortBy { get; set; }
    }
}
