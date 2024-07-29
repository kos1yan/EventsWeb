using Events.Application.DataTransferObjects.Member;
using FluentValidation;

namespace Events.Application.Validators
{
    public class MemberForCreationDtoValidator : AbstractValidator<MemberForCreationDto>
    {
        public MemberForCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name is required!")
                .MaximumLength(30).WithMessage("Name cannot exceed 30+ characters!");
            RuleFor(x => x.Surname).NotNull().WithMessage("Surname is required!")
                .MaximumLength(30).WithMessage("Surname cannot exceed 30+ characters!");
            RuleFor(x => x.DateOfBirth).NotNull().WithMessage("Date Of birth is required!");
            RuleFor(x => x.Email).NotNull().WithMessage("Category is required!")
                .EmailAddress().WithMessage("Email must be valid!");

        }
    }
}
