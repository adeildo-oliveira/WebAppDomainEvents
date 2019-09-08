using FluentValidation;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;

namespace WebAppDomainEvents.Domain.Validations.Salario
{
    public class SalarioCommandBaseValidation<T> : AbstractValidator<T> where T : AddSalarioCommand
    {
        protected SalarioCommandBaseValidation()
        {
            RuleFor(x => x.Pagamento)
                .GreaterThan(0M)
                .WithMessage("O valor Pagamento deve ser maior que zero");

            RuleFor(x => x.Adiantamento)
                .GreaterThan(0M)
                .WithMessage("O valor Adiantamento deve ser maior que zero");
        }
    }
}
