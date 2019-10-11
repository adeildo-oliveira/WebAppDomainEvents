using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DomainEvents.Models;
using WebApi.DomainEvents.Models.CommandsView.SalarioCommandView;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Notifications;

namespace WebApi.DomainEvents.Controllers.v1
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SalarioController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ISalarioRepositoryReadOnly _repository;

        public SalarioController(INotificationHandler<DomainNotification> notifications
            , IMediator mediator
            , IMapper mapper
            , ISalarioRepositoryReadOnly repository) 
            : base(notifications)
        {
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [Route("ObterAsync")]
        public async Task<IActionResult> ObterAsync()
        {
            var salario = await _repository.ObterSalariosAsync();
            var salarioView = _mapper.Map<IReadOnlyCollection<SalarioView>>(salario);

            return Response(salarioView);
        }

        [HttpGet]
        [Route("ObterPorIdAsync/{id:guid}")]
        public async Task<IActionResult> ObterPorIdAsync(Guid id)
        {
            var salario = await _repository.ObterSalarioPorIdAsync(id);
            var salarioView = _mapper.Map<SalarioView>(salario);

            return Response(salarioView);
        }

        [HttpPost]
        [Route("AdicionarAsync")]
        public async Task<IActionResult> AdicionarAsync([FromBody] AddSalarioCommandView salarioCommand)
        {
            var salario = _mapper.Map<AddSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);

            return Response("Salário adicionado com sucesso.");
        }

        [HttpPut]
        [Route("AtualizarAsync")]
        public async Task<IActionResult> AtualizarAsync([FromBody] EditSalarioCommandView salarioCommand)
        {
            var salario = _mapper.Map<EditSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);

            return Response("Salário atualizado com sucesso.");
        }

        [HttpDelete]
        [Route("DeletarAsync")]
        public async Task<IActionResult> DeletarAsync([FromBody] DeleteSalarioCommandView salarioCommand)
        {
            var salario = _mapper.Map<DeleteSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);

            return Response("Salário excluído com sucesso.");
        }
    }
}
