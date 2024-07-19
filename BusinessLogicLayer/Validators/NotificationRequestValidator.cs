using FluentValidation;
using Shared.RequestFeatures;

namespace BusinessLogicLayer.Validators
{
    public class NotificationRequestValidator : AbstractValidator<NotificationRequest>
    {
        public NotificationRequestValidator()
        {
            RuleFor(s => s.Title).NotNull().WithMessage("Title is required!");
            RuleFor(s => s.Body).NotNull().WithMessage("Body is required!");
            RuleFor(s => s.EventId).NotNull().WithMessage("EventId is required!");
        }
    }
}
