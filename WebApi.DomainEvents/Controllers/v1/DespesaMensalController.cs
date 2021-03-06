﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
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
        const string ROTA_LOGUE = "v1/api/DespesaMensal";

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
        [Route("GetAsync")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                _logger.Information($"[DespesaMensalController] OBTENDO OS DADOS DA BASE :: {ROTA_LOGUE}/ObterAsync");
                var despesaMensal = await _repository.ObterDespesasMensaisAsync();
                var despesaMensalView = _mapper.Map<IReadOnlyCollection<DespesaMensalView>>(despesaMensal);
                _logger.Information($"[DespesaMensalController] RETORNO DA CONSULTA :: {ROTA_LOGUE}/ObterAsync");

                return despesaMensalView.Count > 0 ? Response(despesaMensalView) : Response(despesaMensalView, HttpStatusCode.NotFound);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[DespesaMensalController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetByIdAsync/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.Information($"[DespesaMensalController] OBTENDO OS DADOS DA BASE :: {ROTA_LOGUE}/ObterAsync/{id}");
                var despesaMensal = await _repository.ObterDespesaMensalPorIdAsync(id);
                var despesaMensalView = _mapper.Map<DespesaMensalView>(despesaMensal);
                _logger.Information($"[DespesaMensalController] RETORNO DA CONSULTA :: {ROTA_LOGUE}/ObterAsync/{id}");

                return despesaMensalView != null ? Response(despesaMensalView) : Response(despesaMensalView, HttpStatusCode.NotFound);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[DespesaMensalController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("PostAsync")]
        public async Task<IActionResult> PostAsync([FromBody] AddDespesaMensalCommandView commandView)
        {
            try
            {
                _logger.Information($"[DespesaMensalController] INICIANDO A EXECUAÇÃO DO CADASTRO :: {ROTA_LOGUE}/AdicionarAsync/{JsonConvert.SerializeObject(commandView)}");
                var despesaMensal = _mapper.Map<AddDespesaMensalCommand>(commandView);
                var sucess = await _mediator.Send(despesaMensal);
                _logger.Information("[DespesaMensalController] FIM DA EXECUÇÃO DO CADASTRO");

                return sucess ? Response("Despesa mensal adicionado com sucesso.") : Response(statusCode: HttpStatusCode.BadRequest);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[DespesaMensalController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("PutAsync")]
        public async Task<IActionResult> PutAsync([FromBody] EditDespesaMensalCommandView commandView)
        {
            try
            {
                _logger.Information($"[DespesaMensalController] INICIANDO A EXECUAÇÃO DE EDIÇÃO :: {ROTA_LOGUE}/AtualizarAsync/{JsonConvert.SerializeObject(commandView)}");
                var despesaMensal = _mapper.Map<EditDespesaMensalCommand>(commandView);
                var sucess = await _mediator.Send(despesaMensal);
                _logger.Information("[DespesaMensalController] FIM DA EXECUAÇÃO DE EDIÇÃO");

                return sucess ? Response("Despesa mensal atualizada com sucesso.") : Response(statusCode: HttpStatusCode.BadRequest);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[DespesaMensalController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteDespesaMensalCommandView commandView)
        {
            try
            {
                _logger.Information($"[DespesaMensalController] INICIANDO A EXECUAÇÃO DE EXCLUSÃO :: {ROTA_LOGUE}/DeletarAsync/{JsonConvert.SerializeObject(commandView)}");
                var despesaMensal = _mapper.Map<DeleteDespesaMensalCommand>(commandView);
                var sucess = await _mediator.Send(despesaMensal);
                _logger.Information("[DespesaMensalController] FIM DA EXECUAÇÃO DE EXCLUSÃO");

                return sucess ? Response("Despesa mensal removida com sucesso.") : Response(statusCode: HttpStatusCode.BadRequest);
            }
            catch (Exception erro)
            {
                _logger.Error(erro, erro.Message);
                NotifyError("500", "[DespesaMensalController] Erro ao processar a requisição");
                return Response(statusCode: HttpStatusCode.InternalServerError);
            }
        }
    }
}
