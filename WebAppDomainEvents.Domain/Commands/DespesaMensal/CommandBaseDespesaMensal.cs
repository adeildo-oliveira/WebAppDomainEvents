﻿using System;

namespace WebAppDomainEvents.Domain.Commands.DespesaMensal
{
    public abstract class CommandBaseDespesaMensal : Command
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public bool Status { get; set; }
    }
}
