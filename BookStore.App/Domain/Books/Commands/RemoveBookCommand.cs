using System.ComponentModel.DataAnnotations;

namespace BookStore.App.Books
{
    /// <summary>
    /// Represents a user request to remove a book from the system.
    /// </summary>
    public class RemoveBookCommand
    {
        public long Id { get; set; }
    }
}