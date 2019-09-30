using System;

namespace WebApi.DomainEvents.Models
{
    public class DespesaMensalView
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public bool Status { get; set; }
        public SalarioView SalarioView { get; set; }
    }
}