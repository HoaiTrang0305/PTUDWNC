using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class CategoryValidation: AbstractValidator<CategoryEditModel>
    {
      public CategoryValidation() {
            RuleFor(a => a.Name)
                   .NotEmpty()
                   .WithMessage("Tên tác giả không được để trống")
                   .MaximumLength(100)
                   .WithMessage("Tên tác giả tối đa 100 kí tự");

            RuleFor(a => a.UrlSlug)
                .NotEmpty()
                .WithMessage("UrlSlug không được để trống")
                .MaximumLength(100)
                .WithMessage("UrlSlug tối đa 100 kí tự");



            RuleFor(a => a.Description)
                .MaximumLength(100)
                .WithMessage("Ghi chú tối đa 500 kí tự");
        }
    }
}
