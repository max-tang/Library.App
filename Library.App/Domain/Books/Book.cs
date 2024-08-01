using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Library.App.Books
{
    [Index(nameof(Author)), Index(nameof(Title))]
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateOnly PublishDate { get; set; }

        public string Description { get; set; }
    }
}
