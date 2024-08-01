using FluentValidation;

namespace Library.App.Books
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(command => command.Title).NotNull().NotEmpty().WithMessage("Title is required");
            RuleFor(command => command.Author).NotNull().NotEmpty().WithMessage("Author is required");
            RuleFor(command => command.Isbn).NotNull().Must(isbn => IsbnValidator.IsValid(isbn)).WithMessage("Invalid ISBN");
        }
    }
}
