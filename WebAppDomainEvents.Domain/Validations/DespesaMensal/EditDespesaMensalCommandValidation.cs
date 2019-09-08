using FluentValidation;
using System;
using WebAppDomainEvents.Domain.Commands.DespesaMensal;

namespace WebAppDomainEvents.Domain.Validations.DespesaMensal
{
    public class EditDespesaMensalCommandValidation : DespesaMensalCommandBaseValidation<EditDespesaMensalCommand>
    {
        public EditDespesaMensalCommandValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id despesa mensal inválido");
        }
    }
}
