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
using Serilog;

namespace WebApi.DomainEvents.Controllers.v1
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class SalarioController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ISalarioRepositoryReadOnly _repository;
        const string ROTA_LOGUE = "v1/api/salario";

        public SalarioController(INotificationHandler<DomainNotification> notifications
            , ILogger logger
            , IMediator mediator
            , IMapper mapper
            , ISalarioRepositoryReadOnly repository) 
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
            try
            {
                _logger.Information(messageTemplate: $"{ROTA_LOGUE}/ObterAsync - OBTENDO OS DADOS DA BASE");
                var salario = await _repository.ObterSalariosAsync();
                var salarioView = _mapper.Map<IReadOnlyCollection<SalarioView>>(salario);
                _logger.Information(messageTemplate: $"{ROTA_LOGUE}/ObterAsync - RETORNO DA CONSULTA");

                return Response(salarioView);
            }
            catch (Exception erro)
            {
                _logger.Error(erro.Message, erro);
                return Response("Erro ao processar a requisição");
            }
        }

        [HttpGet]
        [Route("ObterPorIdAsync/{id:guid}")]
        public async Task<IActionResult> ObterPorIdAsync(Guid id)
        {
            _logger.Information(messageTemplate: $"{ROTA_LOGUE}/ObterPorIdAsync/{id} - OBTENDO OS DADOS DA BASE");
            var salario = await _repository.ObterSalarioPorIdAsync(id);
            var salarioView = _mapper.Map<SalarioView>(salario);
            _logger.Information(messageTemplate: $"{ROTA_LOGUE}/ObterPorIdAsync/{id} - RETORNO DA CONSULTA");

            return Response(salarioView);
        }

        [HttpPost]
        [Route("AdicionarAsync")]
        public async Task<IActionResult> AdicionarAsync([FromBody] AddSalarioCommandView salarioCommand)
        {
            _logger.Information(messageTemplate: $"{ROTA_LOGUE}/AdicionarAsync - INICIANDO A EXECUAÇÃO DO CADASTRO", propertyValue: salarioCommand);
            var salario = _mapper.Map<AddSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);
            _logger.Information(messageTemplate: $"{ROTA_LOGUE}/AdicionarAsync - FIM DA EXECUÇÃO DO CADASTRO", propertyValue: salarioCommand);

            return Response("Salário adicionado com sucesso.");
        }

        [HttpPut]
        [Route("AtualizarAsync")]
        public async Task<IActionResult> AtualizarAsync([FromBody] EditSalarioCommandView salarioCommand)
        {
            _logger.Information(messageTemplate: $"{ROTA_LOGUE}/AtualizarAsync - INICIANDO A EXECUAÇÃO DE EDIÇÃO");
            var salario = _mapper.Map<EditSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);
            _logger.Information(messageTemplate: $"{ROTA_LOGUE}/AtualizarAsync - FIM DA EXECUAÇÃO DE EDIÇÃO");

            return Response("Salário atualizado com sucesso.");
        }

        [HttpDelete]
        [Route("DeletarAsync")]
        public async Task<IActionResult> DeletarAsync([FromBody] DeleteSalarioCommandView salarioCommand)
        {
            _logger.Information(messageTemplate: $"{ROTA_LOGUE}/DeletarAsync - INICIANDO A EXECUAÇÃO DE EXCLUSÃO");
            var salario = _mapper.Map<DeleteSalarioCommand>(salarioCommand);
            await _mediator.Send(salario);
            _logger.Information(messageTemplate: $"{ROTA_LOGUE}/AtualizarAsync - FIM DA EXECUAÇÃO DE EXCLUSÃO");

            return Response("Salário excluído com sucesso.");
        }
    }
}
