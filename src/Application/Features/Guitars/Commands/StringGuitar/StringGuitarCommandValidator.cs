namespace Application.Features.Guitars.Commands.StringGuitar;

public class StringGuitarCommandValidator : AbstractValidator<StringGuitarCommand>
{
    private readonly GuitarsContext _guitarContext;

    public StringGuitarCommandValidator(GuitarsContext guitarsContext)
    {
        _guitarContext = guitarsContext;

        var validStringTunings = new List<string> { "Ab", "A", "A#", "Bb", "B", "C", "C#", "Db", "D", "D#", "Eb", "E", "F", "F#", "Gb", "G", "G#" };

        RuleFor(x => x.Strings).NotEmpty().WithMessage("Strings collection cannot be empty.");
        RuleFor(x => x).MustAsync(HaveValidStringNumbers).WithMessage("Strings collection has invalid numbers.");
        RuleForEach(x => x.Strings)
            .ChildRules(c => c.RuleFor(cr => cr.Number).GreaterThan(0).WithMessage("Number must be greater than 0."))
            .ChildRules(c => c.RuleFor(cr => cr.Gauge).NotEmpty().WithMessage("Gauge is required."))
            .ChildRules(c => c.RuleFor(cr => cr.Tuning).NotEmpty().WithMessage("Tuning is required.")
                .Must(x => validStringTunings.Contains(x)).WithMessage($"Tuning must be one of the following values: {string.Join(",", validStringTunings)}"));
    }

    protected override bool PreValidate(ValidationContext<StringGuitarCommand> context, ValidationResult result)
    {
        if (context.InstanceToValidate.Strings == null)
        {
            result.Errors.Add(new ValidationFailure("Strings", "Strings collection cannot be null."));
            return false;
        }

        return base.PreValidate(context, result);
    }

    private async Task<bool> HaveValidStringNumbers(StringGuitarCommand guitarStringCommand, CancellationToken cancellationToken)
    {
        var guitar = await _guitarContext.Guitar.FindAsync(guitarStringCommand.Id);
        if (guitar == null)
        {
            throw new NotFoundException(nameof(Guitar), guitarStringCommand.Id);
        }

        var hasInvalidStringNumbers = guitarStringCommand.Strings.Any(x => x.Number > guitar.MaxNumberOfStrings);
        return !hasInvalidStringNumbers;
    }
}