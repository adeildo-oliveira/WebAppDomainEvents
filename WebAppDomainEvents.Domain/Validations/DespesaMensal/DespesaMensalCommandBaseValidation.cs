using FluentValidation;
using System;
using WebAppDomainEvents.Domain.Commands.DespesaMensalCommand;

namespace WebAppDomainEvents.Domain.Validations.DespesaMensal
{
    public class DespesaMensalCommandBaseValidation : AbstractValidator<CommandBaseDespesaMensal>
    {
        protected void RuleId() => RuleFor(c => c.Id)
                        .NotEqual(Guid.Empty)
                        .WithMessage("Id despesa mensal inválido");
        protected void RuleIdSalario() => RuleFor(c => c.IdSalario)
                        .NotEqual(Guid.Empty)
                        .WithMessage("Id salário inválido");
        protected void RuleDescricao() => RuleFor(x => x.Descricao)
                        .NotEmpty().When(m => string.IsNullOrWhiteSpace(m.Descricao))
                        .WithMessage("Descrição inválida");
        protected void RuleValor() => RuleFor(x => x.Valor)
                        .GreaterThan(0M)
                        .WithMessage("O valor Pagamento deve ser maior que zero");
        protected void RuleData() => RuleFor(x => x.Data)
                        .Must(ValidDate)
                        .WithMessage("Data inválida");
        protected void RuleStatus() => RuleFor(c => c.Status)
                        .Must(ValidateBoolean)
                        .WithMessage("Campo status inválido para exclusão");
        private bool ValidDate(DateTime date) => !date.Equals(default);
        private bool ValidateBoolean(bool property) => !property;
    }
}
