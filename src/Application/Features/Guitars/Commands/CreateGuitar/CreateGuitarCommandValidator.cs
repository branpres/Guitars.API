using FluentValidation;

namespace Application.Features.Guitars.Commands.CreateGuitar
{
    public class CreateGuitarCommandValidator : AbstractValidator<CreateGuitarCommand>
    {
        public CreateGuitarCommandValidator()
        {
            RuleFor(x => x.GuitarType).IsInEnum().WithMessage("Guitar Type is invalid.");
            RuleFor(x => x.MaxNumberOfStrings).GreaterThan(0).WithMessage("Max number of strings must be greater than 0.");
            RuleFor(x => x.Make).NotEmpty().WithMessage("Make is required.");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required.");
        }
    }
}