using System;

namespace WebApi.DomainEvents.Models.CommandsView.DespesaMensalView
{
    public class EditDespesaMensalCommandView
    {
        public Guid Id { get; set; }
        public Guid IdSalario { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
