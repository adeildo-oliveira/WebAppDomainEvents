using FluentValidation;
using System;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;

namespace WebAppDomainEvents.Domain.Validations.Salario
{
    public class EditSalarioCommandValidation : SalarioCommandBaseValidation<EditSalarioCommand>
    {
        public EditSalarioCommandValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id salário inválido");
        }
    }
}
