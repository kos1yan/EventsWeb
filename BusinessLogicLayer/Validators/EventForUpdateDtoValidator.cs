using FluentValidation;
using Shared.DataTransferObjects.Event;

namespace BusinessLogicLayer.Validators
{
    public class EventForUpdateDtoValidator : AbstractValidator<EventForUpdateDto>
    {
        public EventForUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name is required!")
                .MaximumLength(30).WithMessage("Name cannot exceed 30+ characters!");
            RuleFor(x => x.Description).NotNull().WithMessage("Description is required!")
                .MaximumLength(200).WithMessage("Description cannot exceed 200+ characters!");
            RuleFor(x => x.Adress).NotNull().WithMessage("Adress is required!")
                .MaximumLength(50).WithMessage("Adress cannot exceed 50+ characters!");
            RuleFor(x => x.Date).NotNull().WithMessage("Date is required!");
            RuleFor(x => x.MaxMemberCount).NotNull().WithMessage("Member count is required!");
            RuleFor(x => x.CategoryId).NotNull().WithMessage("Category is required!");
        }
    }
}
