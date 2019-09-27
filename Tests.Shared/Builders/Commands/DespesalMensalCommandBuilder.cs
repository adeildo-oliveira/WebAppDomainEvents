using System;
using WebAppDomainEvents.Domain.Commands.DespesaMensal;

namespace Tests.Shared.Builders.Commands
{
    public class AddDespesaMensalCommandBuilder : InMemoryBuilder<AddDespesaMensalCommand>
    {
        private string _descricao;
        private decimal _valor;
        private DateTime _data;

        public AddDespesaMensalCommandBuilder ComDescricao(string descricao)
        {
            _descricao = descricao;
            return this;
        }

        public AddDespesaMensalCommandBuilder ComValor(Decimal valor)
        {
            _valor = valor;
            return this;
        }

        public AddDespesaMensalCommandBuilder ComData(DateTime data)
        {
            _data = data;
            return this;
        }

        public override AddDespesaMensalCommand Instanciar() => new AddDespesaMensalCommand
        {
            Descricao = _descricao,
            Valor = _valor,
            Data = _data
        };
    }

    public class EditDespesaMensalCommandBuilder : InMemoryBuilder<EditDespesaMensalCommand>
    {
        private Guid _id;
        private string _descricao;
        private decimal _valor;
        private DateTime _data;

        public EditDespesaMensalCommandBuilder ComId(Guid id)
        {
            _id = id;
            return this;
        }

        public EditDespesaMensalCommandBuilder ComDescricao(string descricao)
        {
            _descricao = descricao;
            return this;
        }

        public EditDespesaMensalCommandBuilder ComValor(Decimal valor)
        {
            _valor = valor;
            return this;
        }

        public EditDespesaMensalCommandBuilder ComData(DateTime data)
        {
            _data = data;
            return this;
        }

        public override EditDespesaMensalCommand Instanciar() => new EditDespesaMensalCommand
        {
            Id = _id,
            Descricao = _descricao,
            Valor = _valor,
            Data = _data
        };
    }

    public class DeleteDespesaMensalCommandBuilder : InMemoryBuilder<DeleteDespesaMensalCommand>
    {
        private Guid _id;
        private bool _status;

        public DeleteDespesaMensalCommandBuilder ComId(Guid id)
        {
            _id = id;
            return this;
        }

        public DeleteDespesaMensalCommandBuilder ComStatus(bool status)
        {
            _status = status;
            return this;
        }

        public override DeleteDespesaMensalCommand Instanciar() => new DeleteDespesaMensalCommand
        {
            Id = _id,
            Status = _status
        };
    }
}
