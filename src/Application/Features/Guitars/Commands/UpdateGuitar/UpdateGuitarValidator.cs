using FluentValidation;

namespace Application.Features.Guitars.Commands.UpdateGuitar
{
    public class UpdateGuitarValidator : AbstractValidator<UpdateGuitar>
    {
        public UpdateGuitarValidator()
        {
            RuleFor(x => x.Make).NotEmpty().WithMessage("Make is required.");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required.");
        }
    }
}