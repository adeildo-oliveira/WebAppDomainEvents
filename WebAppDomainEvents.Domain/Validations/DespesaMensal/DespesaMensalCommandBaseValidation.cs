using FluentValidation;
using System;
using WebAppDomainEvents.Domain.Commands.DespesaMensal;

namespace WebAppDomainEvents.Domain.Validations.DespesaMensal
{
    public class DespesaMensalCommandBaseValidation<T> : AbstractValidator<T> where T : AddDespesaMensalCommand
    {
        protected DespesaMensalCommandBaseValidation()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().When(m => string.IsNullOrWhiteSpace(m.Descricao))
                .WithMessage("Descrição inválida");

            RuleFor(x => x.Valor)
                .GreaterThan(0M)
                .WithMessage("O valor Pagamento deve ser maior que zero");

            RuleFor(x => x.Data)
                .Must(ValidDate)
                .WithMessage("A data inválida");
        }

        private bool ValidDate(DateTime date) => !date.Equals(default);
    }
}
