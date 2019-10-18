using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DomainEvents.Controllers.v1;
using WebApi.DomainEvents.Models;
using WebApi.DomainEvents.Models.CommandsView.DespesaMensalView;
using WebAppDomainEvents.Domain.Commands.DespesaMensalCommand;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Domain.Notifications;
using Xunit;
using Serilog;

namespace Tests.Unit.WebApi.v1
{
    public class DespesaMensalControllerTests
    {
        private readonly AutoMocker _mocker;
        private readonly DespesaMensalController _controller;
        private readonly Mock<DomainNotificationHandler> _notificationMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IDespesaMensalRepositoryReadOnly> _repositoryMock;
        private readonly Mock<ILogger> _loggerMock;

        public DespesaMensalControllerTests()
        {
            _mocker = new AutoMocker();
            _notificationMock = _mocker.GetMock<DomainNotificationHandler>();
            _mediatorMock = _mocker.GetMock<IMediator>();
            _mapperMock = _mocker.GetMock<IMapper>();
            _repositoryMock = _mocker.GetMock<IDespesaMensalRepositoryReadOnly>();
            _loggerMock = _mocker.GetMock<ILogger>();

            _controller = new DespesaMensalController(_notificationMock.Object,
                _loggerMock.Object,
                _mediatorMock.Object,
                _mapperMock.Object,
                _repositoryMock.Object);
        }

        [Fact]
        public async Task DespesaMensalObterDespesaMensalAsync()
        {
            var despesaMensal = new Mock<IReadOnlyCollection<DespesaMensal>>().Object;
            var despesaMensalView = new Mock<IReadOnlyCollection<DespesaMensalView>>().Object;

            _repositoryMock.Setup(x => x.ObterDespesasMensaisAsync()).ReturnsAsync(despesaMensal);
            _mapperMock.Setup(x => x.Map<IReadOnlyCollection<DespesaMensalView>>(despesaMensal)).Returns(despesaMensalView);

            var viewResult = (await _controller.ObterAsync()) as OkObjectResult;

            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalObterDespesaMensalPorIdAsync()
        {
            var despesaMensal = new Mock<DespesaMensal>().Object;
            var despesaMensalView = new Mock<DespesaMensalView>().Object;

            _repositoryMock.Setup(x => x.ObterDespesaMensalPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(despesaMensal);
            _mapperMock.Setup(x => x.Map<DespesaMensalView>(despesaMensal)).Returns(despesaMensalView);

            var viewResult = (await _controller.ObterAsync()) as OkObjectResult;

            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalNaoAdicionarAsync()
        {
            var commandModel = new Mock<AddDespesaMensalCommandView>().Object;
            var command = new Mock<AddDespesaMensalCommand>().Object;

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<AddDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(false);

            var viewResult = (await _controller.AdicionarAsync(commandModel)) as BadRequestObjectResult;
            viewResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalAdicionarAsync()
        {
            var commandModel = new AddDespesaMensalCommandView
            {
                IdSalario = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Descricao = "Cartão de Crédito",
                Valor = decimal.One,
                Data = DateTime.Now
            };
            var command = new Mock<AddDespesaMensalCommand>().Object;

            _notificationMock.Setup(x => x.HasNotifications()).Returns(false);
            _mapperMock.Setup(x => x.Map<AddDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(true);

            var viewResult = (await _controller.AdicionarAsync(commandModel)) as OkObjectResult;
            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalNaoAtualizarAsync()
        {
            var commandModel = new EditDespesaMensalCommandView
            {
                IdSalario = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Descricao = "Cartão de Crédito",
                Valor = decimal.One,
                Data = DateTime.Now
            };
            var command = new Mock<EditDespesaMensalCommand>().Object;

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<EditDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(false);

            var viewResult = (await _controller.AtualizarAsync(commandModel)) as BadRequestObjectResult;
            viewResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalAtualizarAsync()
        {
            var commandModel = new EditDespesaMensalCommandView
            {
                Id = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                IdSalario = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Descricao = "Cartão de Crédito",
                Valor = decimal.One,
                Data = DateTime.Now
            };
            var command = new Mock<EditDespesaMensalCommand>().Object;

            _notificationMock.Setup(x => x.HasNotifications()).Returns(false);
            _mapperMock.Setup(x => x.Map<EditDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(true);

            var viewResult = (await _controller.AtualizarAsync(commandModel)) as OkObjectResult;
            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalNaoDeletarAsync()
        {
            var commandModel = new Mock<DeleteDespesaMensalCommandView>().Object;
            var command = new Mock<DeleteDespesaMensalCommand>().Object;

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<DeleteDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(false);

            var viewResult = (await _controller.DeletarAsync(commandModel)) as BadRequestObjectResult;
            viewResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalDeletarAsync()
        {
            var commandModel = new DeleteDespesaMensalCommandView
            {
                Id = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                IdSalario = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Status = false
            };
            var command = new Mock<DeleteDespesaMensalCommand>().Object;

            _notificationMock.Setup(x => x.HasNotifications()).Returns(false);
            _mapperMock.Setup(x => x.Map<DeleteDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(true);

            var viewResult = (await _controller.DeletarAsync(commandModel)) as OkObjectResult;
            viewResult.Should().BeOfType<OkObjectResult>();
        }
    }
}
