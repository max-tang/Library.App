using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookStore.App.Books
{
    public class UpdateBookCommand
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Isbn { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }


        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly PublishDate { get; set; }

        public string Description { get; set; }
    }
}
