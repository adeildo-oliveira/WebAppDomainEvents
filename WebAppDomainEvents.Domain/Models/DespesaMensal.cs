using System;

namespace WebAppDomainEvents.Domain.Models
{
    public class DespesaMensal : Entity
    {
        public DespesaMensal() { }

        public DespesaMensal(string descricao, decimal valor, DateTime data, bool status = true)
        {
            Descricao = descricao;
            Valor = valor;
            Data = data;
            Status = status;
        }

        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; } = DateTime.Now;
        public bool Status { get; private set; }
        public Salario Salario { get; private set; }

        public virtual DespesaMensal AdicionarSalario(Salario salario)
        {
            Salario = salario;
            return this;
        }
    }
}
