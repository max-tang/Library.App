
using FluentValidation;

namespace Library.App.Books
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(command => command.Id).NotNull().GreaterThan(0).WithMessage("Invalid book id");
            RuleFor(command => command.Title).NotNull().NotEmpty().WithMessage("Title is required");
            RuleFor(command => command.Author).NotNull().NotEmpty().WithMessage("Author is required");
            RuleFor(command => command.Isbn).Must(isbn => IsbnValidator.IsValid(isbn)).WithMessage("Invalid ISBN");
        }
    }
}

