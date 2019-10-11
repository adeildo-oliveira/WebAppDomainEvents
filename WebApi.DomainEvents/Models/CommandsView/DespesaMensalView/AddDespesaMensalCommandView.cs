using System;

namespace WebApi.DomainEvents.Models.CommandsView.DespesaMensalView
{
    public class AddDespesaMensalCommandView
    {
        public Guid IdSalario { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
