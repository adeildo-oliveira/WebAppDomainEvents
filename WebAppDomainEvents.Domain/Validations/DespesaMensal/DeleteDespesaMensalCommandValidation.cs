using FluentValidation;
using System;
using WebAppDomainEvents.Domain.Commands.DespesaMensal;

namespace WebAppDomainEvents.Domain.Validations.DespesaMensal
{
    public class DeleteDespesaMensalCommandValidation : AbstractValidator<DeleteDespesaMensalCommand>, IValidationMethods
    {
        public DeleteDespesaMensalCommandValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id despesa mensal inválido");

            RuleFor(c => c.Status)
                .Must(ValidateBoolean)
                .WithMessage("O campo status deve ser informado");
        }

        public bool ValidateBoolean(bool property) => !property;
    }
}
