using BookmarkManagerApp.Controllers.Requests;
using FluentValidation;

namespace BookmarkManagerApp.Controllers.Validators;

public class CreateVisitRequestValidator : DefaultRequestValidator<CreateVisitRequest>
{
    public CreateVisitRequestValidator()
    {
        RuleFor(x => x.BookmarkId).GreaterThan(0);
        RuleFor(x => x.VisitTime).GreaterThan(DateTimeOffset.UtcNow.AddYears(-1))
            .WithMessage("'Visit Time' must be within the last year or in the future.");
    }
}