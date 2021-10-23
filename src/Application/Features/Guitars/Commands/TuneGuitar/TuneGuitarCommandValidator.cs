using Application.Common.Exceptions;
using Application.Data;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Features.Guitars.Commands.TuneGuitar
{
    public class TuneGuitarCommandValidator : AbstractValidator<TuneGuitarCommand>
    {
        private readonly GuitarsContext _guitarContext;

        public TuneGuitarCommandValidator(GuitarsContext guitarsContext)
        {
            _guitarContext = guitarsContext;

            var validStringTunings = new List<string> { "Ab", "A", "A#", "Bb", "B", "C", "C#", "Db", "D", "D#", "Eb", "E", "F", "F#", "Gb", "G", "G#" };

            RuleFor(x => x.Tunings).NotEmpty().WithMessage("Tunings collection cannot be empty.");
            RuleFor(x => x).MustAsync(HaveValidStringNumbers).WithMessage("Tunings collection has invalid numbers.");
            RuleForEach(x => x.Tunings)
                .ChildRules(c => c.RuleFor(cr => cr.Number).GreaterThan(0).WithMessage("Number must be greater than 0."))
                .ChildRules(c => c.RuleFor(cr => cr.Tuning).NotEmpty().WithMessage("Tuning is required.")
                    .Must(x => validStringTunings.Contains(x)).WithMessage($"Tuning must be one of the following values: {string.Join(",", validStringTunings)}"));
        }


        protected override bool PreValidate(ValidationContext<TuneGuitarCommand> context, ValidationResult result)
        {
            if (context.InstanceToValidate.Tunings == null)
            {
                result.Errors.Add(new ValidationFailure("Tunings", "Tunings collection cannot be null."));
                return false;
            }

            return base.PreValidate(context, result);
        }

        private async Task<bool> HaveValidStringNumbers(TuneGuitarCommand guitarStringCommand, CancellationToken cancellationToken)
        {
            var guitar = await _guitarContext.Guitar.FindAsync(guitarStringCommand.Id);
            if (guitar == null)
            {
                throw new NotFoundException(nameof(Guitar), guitarStringCommand.Id);
            }

            var hasInvalidStringNumbers = guitarStringCommand.Tunings.Any(x => x.Number > guitar.MaxNumberOfStrings);
            return !hasInvalidStringNumbers;
        }
    }
}
