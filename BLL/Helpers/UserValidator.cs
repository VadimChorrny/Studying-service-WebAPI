using FluentValidation;
using DAL.Entities;

namespace BLL.Helpers
{

    public class UserValidator : AbstractValidator<ApplicationUser>
    {
        public UserValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
            RuleFor(u => u.UserName).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(u => u.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.LastName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.Age).ExclusiveBetween(6, 122);
            RuleFor(u => u.Email).EmailAddress();
        }
    }
}