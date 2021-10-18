using FluentValidation;
using FluentValidation.Results;

namespace Application.Guitars.Queries.ReadGuitarsFiltered
{
    public class ReadGuitarsFilteredQueryValidator : AbstractValidator<ReadGuitarsFilteredQuery>
    {
        public ReadGuitarsFilteredQueryValidator() { }

        protected override bool PreValidate(ValidationContext<ReadGuitarsFilteredQuery> context, ValidationResult result)
        {
            if (context.InstanceToValidate.GuitarType == null
                && context.InstanceToValidate.MaxNumberOfStrings == null
                && context.InstanceToValidate.Make == null
                && context.InstanceToValidate.Model == null)
            {
                result.Errors.Add(new ValidationFailure("Parameters", "A guitar type, max number of strings, make, or model must be specified."));
                return false;
            }

            return base.PreValidate(context, result);
        }
    }
}