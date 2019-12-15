using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
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
        [Route("GetAsync")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                _logger.Information($"[SalarioController] OBTENDO OS DADOS DA BASE :: {ROTA_LOGUE}/ObterAsync");
                var salario = await _repository.ObterSalariosAsync();
                var salarioView = _mapper.Map<IReadOnlyCollection<SalarioView>>(salario);
                _logger.Information($"[SalarioController] RETORNO DA CONSULTA :: {ROTA_LOGUE}/ObterAsync");

                return salarioView.Count > 0 ? Response(salarioView) : Response(salarioView, HttpStatusCode.NotFound);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[SalarioController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetByIdAsync/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.Information($"[SalarioController] OBTENDO OS DADOS DA BASE :: {ROTA_LOGUE}/ObterPorIdAsync/{id}");
                var salario = await _repository.ObterSalarioPorIdAsync(id);
                var salarioView = _mapper.Map<SalarioView>(salario);
                _logger.Information($"[SalarioController] RETORNO DA CONSULTA :: {ROTA_LOGUE}/ObterPorIdAsync/{id}");

                return salarioView != null ? Response(salarioView) : Response(salarioView, HttpStatusCode.NotFound);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[SalarioController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("PostAsync")]
        public async Task<IActionResult> PostAsync([FromBody] AddSalarioCommandView salarioCommand)
        {
            try
            {
                _logger.Information($"[SalarioController] INICIANDO A EXECUAÇÃO DO CADASTRO :: {ROTA_LOGUE}/AdicionarAsync/{JsonConvert.SerializeObject(salarioCommand)}");
                var salario = _mapper.Map<AddSalarioCommand>(salarioCommand);
                var sucess = await _mediator.Send(salario);
                _logger.Information("[SalarioController] FIM DA EXECUÇÃO DO CADASTRO");

                return sucess ? Response("Salário adicionado com sucesso.") : Response(statusCode: HttpStatusCode.BadRequest);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[SalarioController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("PutAsync")]
        public async Task<IActionResult> PutAsync([FromBody] EditSalarioCommandView salarioCommand)
        {
            try
            {
                _logger.Information($"[SalarioController] INICIANDO A EXECUAÇÃO DE EDIÇÃO :: {ROTA_LOGUE}/AtualizarAsync/{JsonConvert.SerializeObject(salarioCommand)}");
                var salario = _mapper.Map<EditSalarioCommand>(salarioCommand);
                var sucess = await _mediator.Send(salario);
                _logger.Information("[SalarioController] FIM DA EXECUAÇÃO DE EDIÇÃO");

                return sucess ? Response("Salário atualizado com sucesso.") : Response(statusCode: HttpStatusCode.BadRequest);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[SalarioController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteSalarioCommandView salarioCommand)
        {
            try
            {
                _logger.Information($"[SalarioController] INICIANDO A EXECUAÇÃO DE EXCLUSÃO :: {ROTA_LOGUE}/DeletarAsync/{JsonConvert.SerializeObject(salarioCommand)}");
                var salario = _mapper.Map<DeleteSalarioCommand>(salarioCommand);
                var sucess = await _mediator.Send(salario);
                _logger.Information("[SalarioController] FIM DA EXECUAÇÃO DE EXCLUSÃO");

                return sucess ? Response("Salário excluído com sucesso.") : Response(statusCode: HttpStatusCode.BadRequest);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[SalarioController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }
    }
}
