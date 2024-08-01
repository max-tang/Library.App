using System.ComponentModel.DataAnnotations;

namespace BookStore.App.Books
{
    /// <summary>
    /// Represents a user request to find a book with specified book Id.
    /// </summary>
    public record FindByIdQuery()
    {
        public long Id { get; set; }
    }
}
