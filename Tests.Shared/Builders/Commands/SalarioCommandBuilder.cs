using System;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;

namespace Tests.Shared.Builders.Commands
{
    public class AddSalarioCommandBuilder : InMemoryBuilder<AddSalarioCommand>
    {
        private decimal _pagamento;
        private decimal _adiantamento;

        public AddSalarioCommandBuilder ComAdiantamento(decimal adiantamento)
        {
            _adiantamento = adiantamento;
            return this;
        }

        public AddSalarioCommandBuilder ComPagamento(decimal pagamento)
        {
            _pagamento = pagamento;
            return this;
        }

        public override AddSalarioCommand Instanciar() => new AddSalarioCommand
        {
            Pagamento = _pagamento,
            Adiantamento = _adiantamento
        };
    }

    public class EditSalarioCommandBuilder : InMemoryBuilder<EditSalarioCommand>
    {
        private Guid _id;
        private decimal _pagamento;
        private decimal _adiantamento;

        public EditSalarioCommandBuilder ComId(Guid id)
        {
            _id = id;
            return this;
        }

        public EditSalarioCommandBuilder ComAdiantamento(decimal adiantamento)
        {
            _adiantamento = adiantamento;
            return this;
        }

        public EditSalarioCommandBuilder ComPagamento(decimal pagamento)
        {
            _pagamento = pagamento;
            return this;
        }

        public override EditSalarioCommand Instanciar() => new EditSalarioCommand
        {
            Id = _id,
            Pagamento = _pagamento,
            Adiantamento = _adiantamento
        };
    }

    public class DeleteSalarioCommandBuilder : InMemoryBuilder<DeleteSalarioCommand>
    {
        private Guid _id;
        private bool _status;

        public DeleteSalarioCommandBuilder ComId(Guid id)
        {
            _id = id;
            return this;
        }

        public DeleteSalarioCommandBuilder ComStatus(bool status)
        {
            _status = status;
            return this;
        }

        public override DeleteSalarioCommand Instanciar() => new DeleteSalarioCommand
        {
            Id = _id,
            Status = _status
        };
    }
}
