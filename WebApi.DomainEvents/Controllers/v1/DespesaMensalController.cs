using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DomainEvents.Models;
using WebApi.DomainEvents.Models.CommandsView.DespesaMensalView;
using WebAppDomainEvents.Domain.Commands.DespesaMensalCommand;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Notifications;

namespace WebApi.DomainEvents.Controllers.v1
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class DespesaMensalController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IDespesaMensalRepositoryReadOnly _repository;

        public DespesaMensalController(INotificationHandler<DomainNotification> notifications
            , ILogger logger
            , IMediator mediator
            , IMapper mapper
            , IDespesaMensalRepositoryReadOnly repository) 
            : base(notifications, logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [Route("ObterAsync")]
        public async Task<IActionResult> ObterAsync()
        {
            var despesaMensal = await _repository.ObterDespesasMensaisAsync();
            var despesaMensalView = _mapper.Map<IReadOnlyCollection<DespesaMensalView>>(despesaMensal);

            return Response(despesaMensalView);
        }

        [HttpGet]
        [Route("ObterPorIdAsync/{id:guid}")]
        public async Task<IActionResult> ObterPorIdAsync(Guid id)
        {
            var despesaMensal = await _repository.ObterDespesaMensalPorIdAsync(id);
            var despesaMensalView = _mapper.Map<DespesaMensalView>(despesaMensal);

            return Response(despesaMensalView);
        }

        [HttpPost]
        [Route("AdicionarAsync")]
        public async Task<IActionResult> AdicionarAsync([FromBody] AddDespesaMensalCommandView commandView)
        {
            var despesaMensal = _mapper.Map<AddDespesaMensalCommand>(commandView);
            await _mediator.Send(despesaMensal);

            return Response("Despesa mensal adicionado com sucesso.");
        }

        [HttpPut]
        [Route("AtualizarAsync")]
        public async Task<IActionResult> AtualizarAsync([FromBody] EditDespesaMensalCommandView commandView)
        {
            var despesaMensal = _mapper.Map<EditDespesaMensalCommand>(commandView);
            await _mediator.Send(despesaMensal);

            return Response("Despesa mensal atualizada com sucesso.");
        }

        [HttpDelete]
        [Route("DeletarAsync")]
        public async Task<IActionResult> DeletarAsync([FromBody] DeleteDespesaMensalCommandView commandView)
        {
            var despesaMensal = _mapper.Map<DeleteDespesaMensalCommand>(commandView);
            await _mediator.Send(despesaMensal);

            return Response("Despesa mensal removida com sucesso.");
        }
    }
}
