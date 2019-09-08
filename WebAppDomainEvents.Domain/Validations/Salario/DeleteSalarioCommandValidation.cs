using FluentValidation;
using System;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;

namespace WebAppDomainEvents.Domain.Validations.Salario
{
    public class DeleteSalarioCommandValidation : AbstractValidator<DeleteSalarioCommand>, IValidationMethods
    {
        public DeleteSalarioCommandValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id salário inválido");
            
            RuleFor(c => c.Status)
                .Must(ValidateBoolean)
                .WithMessage("O campo status deve ser informado");
        }

        public bool ValidateBoolean(bool property) => !property;
    }
}
