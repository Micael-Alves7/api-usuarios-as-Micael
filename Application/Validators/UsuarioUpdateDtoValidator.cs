using APIUsuarios.Application.DTOs;
using FluentValidation;

public class UsuarioUpdateDtoValidator : AbstractValidator<UsuarioUpdateDto>
{
    public UsuarioUpdateDtoValidator()
    {
        RuleFor(u => u.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .Length(3, 100)
            .WithMessage("O nome deve ter entre 3 e 100 caracteres");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .EmailAddress()
            .WithMessage("O email deve ter um formato válido");

        RuleFor(u => u.DataNascimento)
            .NotEmpty()
            .WithMessage("Data de nascimento é obrigatória")
            .Must(Idade)
            .WithMessage("O usuário deve ter pelo menos 18 anos");

        RuleFor(u => u.Telefone)
            .MaximumLength(15)
            .WithMessage("O telefone deve ter no máximo 15 caracteres")
            .When(u => !string.IsNullOrEmpty(u.Telefone));
    }

    private static bool Idade(DateTime data)
    {
        var anos = DateTime.Today.Year - data.Year;
        if (data.Date > DateTime.Today.AddYears(-anos)) anos--;
        return anos >= 18;
    }
}
