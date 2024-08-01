using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BookStore.App;

namespace BookStore.App.Books
{
    /// <summary>
    /// Represents a user request to create a new book.
    /// </summary>
    public class CreateBookCommand
    {
        public string Isbn { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly PublishDate { get; set; }

        public string Description { get; set; }
    }
}
