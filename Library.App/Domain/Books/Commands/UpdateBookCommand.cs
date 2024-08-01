using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library.App.Books
{
    /// <summary>
    /// Represents a user request to update an existing book in the system.
    /// </summary>
    public class UpdateBookCommand
    {
        public long Id { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly PublishDate { get; set; }

        public string Description { get; set; }
    }
}
