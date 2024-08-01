using System.ComponentModel.DataAnnotations;

namespace BookStore.App.Books
{
    public record FindBooksQuery()
    {
        public string SortBy { get; set; }
    }
}
