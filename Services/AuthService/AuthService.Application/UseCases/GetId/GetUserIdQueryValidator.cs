using FluentValidation;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class GetUserIdQueryValidator : AbstractValidator<GetUserIdQuery>
    {
        public GetUserIdQueryValidator()
        {
            RuleFor(x => x.ClaimsPrincipalIdentity)
                .NotNull()
                .WithMessage("ClaimsPrincipalIdentity cannot be null.");
        }
    }
}
