﻿using System;
using WebAppDomainEvents.Domain.Validations.Salario;

namespace WebAppDomainEvents.Domain.Commands.SalarioCommand
{
    public class DeleteSalarioCommand : Command
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteSalarioCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
