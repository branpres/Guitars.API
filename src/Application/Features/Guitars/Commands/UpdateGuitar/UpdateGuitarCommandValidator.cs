using FluentValidation;

namespace Application.Features.Guitars.Commands.UpdateGuitar
{
    public class UpdateGuitarCommandValidator : AbstractValidator<UpdateGuitarCommand>
    {
        public UpdateGuitarCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id is invalid.");
            RuleFor(x => x.Make).NotEmpty().WithMessage("Make is required.");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required.");
        }
    }
}