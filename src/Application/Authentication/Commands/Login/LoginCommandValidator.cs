namespace Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");

        // put LoginCommand validation code here
        //RuleFor(x => x).Must()
    }
}