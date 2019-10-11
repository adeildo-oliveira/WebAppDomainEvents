using System;

namespace WebAppDomainEvents.Domain.Models
{
    public class DespesaMensal : Entity
    {
        protected DespesaMensal() { }

        public DespesaMensal(string descricao, decimal valor, DateTime data, bool status = true)
        {
            Descricao = descricao;
            Valor = valor;
            Data = data;
            Status = status;
        }
        
        public DespesaMensal(Guid id, string descricao, decimal valor, DateTime data, bool status)
        {
            Id = id;
            Descricao = descricao;
            Valor = valor;
            Data = data;
            Status = status;
        }

        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; }
        public bool Status { get; private set; }
        public Salario Salario { get; private set; }

        public virtual DespesaMensal AdicionarSalario(Salario salario)
        {
            Salario = salario;
            return this;
        }

        public virtual DespesaMensal AtualizarDespesaMensal(string descricao, decimal valor, DateTime data, bool status = true)
        {
            Descricao = descricao;
            Valor = valor;
            Data = data;
            Status = status;

            return this;
        }

        public virtual DespesaMensal AtualizarDespesaMensal(bool status)
        {
            Status = status;
            return this;
        }
    }
}
