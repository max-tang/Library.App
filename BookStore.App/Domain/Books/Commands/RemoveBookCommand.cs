using System.ComponentModel.DataAnnotations;

namespace BookStore.App.Books
{
    public class RemoveBookCommand
    {
        [Required]
        public long Id { get; set; }
    }
}