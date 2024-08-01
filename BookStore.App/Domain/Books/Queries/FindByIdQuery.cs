using System.ComponentModel.DataAnnotations;

namespace BookStore.App.Books
{
    public record FindByIdQuery()
    {
        public long Id { get; set; }
    }
}
