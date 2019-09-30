using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DomainEvents.Models;
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
        private readonly ISalarioRepository _repository;

        public SalarioController(INotificationHandler<DomainNotification> notifications
            , IMediator mediator
            , IMapper mapper
            , ISalarioRepository repository) 
            : base(notifications)
        {
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [Route("ObterSalario")]
        public async Task<IActionResult> ObterSalario()
        {
            var salario = await _repository.ObterSalarioAsync();
            var salarioView = _mapper.Map<IReadOnlyCollection<SalarioView>>(salario);

            return Response(salarioView);
        }

        
        [HttpGet]
        [Route("ObterSalario/{id:guid}")]
        public async Task<IActionResult> ObterSalarioPorId(Guid id)
        {
            var salario = await _repository.ObterSalarioPorIdAsync(id);
            var salarioView = _mapper.Map<SalarioView>(salario);

            return Response(salarioView);
        }

        [HttpPost]
        [Route("PostAdicionar")]
        public async Task<IActionResult> PostAdicionar([FromBody] AddSalarioCommandView salarioCommand)
        {
            var salario = _mapper.Map<AddSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);

            return Response("Salário adicionado com sucesso.");
        }

        [HttpPut]
        [Route("PutEditar")]
        public async Task<IActionResult> PutAdicionar([FromBody] EditSalarioCommandView salarioCommand)
        {
            var salario = _mapper.Map<EditSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);

            return Response("Salário atualizado com sucesso.");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteSalarioCommandView salarioCommand)
        {
            var salario = _mapper.Map<DeleteSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);

            return Response("Salário excluído com sucesso.");
        }
    }
}
