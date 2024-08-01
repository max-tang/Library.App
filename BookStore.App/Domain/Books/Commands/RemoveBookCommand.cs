using System.ComponentModel.DataAnnotations;

namespace BookStore.App.Books
{
    /// <summary>
    /// Represents a user request to remove a book from the system.
    /// </summary>
    public class RemoveBookCommand
    {
        [Required]
        public long Id { get; set; }
    }
}