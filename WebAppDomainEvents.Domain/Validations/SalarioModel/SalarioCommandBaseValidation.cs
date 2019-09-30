using FluentValidation;
using System;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;

namespace WebAppDomainEvents.Domain.Validations.SalarioModel
{
    public abstract class SalarioCommandBaseValidation : AbstractValidator<CommandBaseSalario>
    {
        protected virtual void RuleId() => RuleFor(c => c.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Id salário inválido");

        protected virtual void RulePagamento() => RuleFor(x => x.Pagamento)
                        .GreaterThan(0M)
                        .WithMessage("O valor Pagamento deve ser maior que zero");

        protected virtual void RuleAdiantamento() => RuleFor(x => x.Adiantamento)
                        .GreaterThan(0M)
                        .WithMessage("O valor Adiantamento deve ser maior que zero");

        protected void RuleStatus() => RuleFor(c => c.Status)
            .Must(ValidateBoolean)
            .WithMessage("Campo status inválido para exclusão");

        private bool ValidateBoolean(bool property) => !property;
    }
}
