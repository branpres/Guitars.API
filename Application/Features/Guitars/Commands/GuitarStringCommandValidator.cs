using Application.Common.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Guitars.Commands
{
    public class GuitarStringCommandValidator : AbstractValidator<GuitarStringCommand>
    {
        private readonly IGuitarsContext _guitarContext;

        public GuitarStringCommandValidator(IGuitarsContext guitarsContext)
        {
            _guitarContext = guitarsContext;            
            
            var validStringTunings = new List<string> { "Ab", "A", "A#", "Bb", "B", "C", "C#", "Db", "D", "D#", "Eb", "E", "F", "F#", "Gb", "G", "G#" };
            
            RuleFor(x => x).NotEmpty().WithMessage("GuitarStrings collection cannot be empty.")
                .MustAsync(HaveValidStringNumbers).WithMessage("");
            RuleForEach(x => x.GuitarStrings)
                .ChildRules(c => c.RuleFor(cr => cr.Number).GreaterThan(0).WithMessage("Number must be greater than 0."))
                .ChildRules(c => c.RuleFor(cr => cr.Tuning).NotEmpty().WithMessage("Tuning is required.")
                    .Must(x => validStringTunings.Contains(x)).WithMessage($"Tuning must be one of the following values: {string.Join(",", validStringTunings)}"));
        }
        

        protected override bool PreValidate(ValidationContext<GuitarStringCommand> context, ValidationResult result)
        {
            if (context.InstanceToValidate.GuitarStrings == null)
            {
                result.Errors.Add(new ValidationFailure("GuitarStrings", "GuitarStrings collection cannot be null."));
                return false;
            }

            return base.PreValidate(context, result);
        }

        private async Task<bool> HaveValidStringNumbers(GuitarStringCommand guitarStringCommand, CancellationToken cancellation)
        {
            var guitar = await _guitarContext.Guitar.FindAsync(guitarStringCommand.Id);
            if (guitar == null)
            {
                return false;
            }

            var invalidStringNumbers = guitarStringCommand.GuitarStrings.Where(x => x.Number > guitar.MaxNumberOfStrings).Select(x => x.Number).Distinct();
            return !invalidStringNumbers.Any();
        }
    }
}