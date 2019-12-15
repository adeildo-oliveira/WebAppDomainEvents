using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Serilog;
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
        public async Task DespesaMensalObterDespesaMensalAsyncOkResult()
        {
            IReadOnlyCollection<DespesaMensal> despesaMensal = new List<DespesaMensal>();
            IReadOnlyCollection<DespesaMensalView> despesaMensalView = new List<DespesaMensalView> { new DespesaMensalView() };

            _repositoryMock.Setup(x => x.ObterDespesasMensaisAsync()).ReturnsAsync(despesaMensal);
            _mapperMock.Setup(x => x.Map<IReadOnlyCollection<DespesaMensalView>>(despesaMensal)).Returns(despesaMensalView);

            var viewResult = (await _controller.GetAsync()) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalObterDespesaMensalAsyncNotFoundResult()
        {
            IReadOnlyCollection<DespesaMensal> despesaMensal = new List<DespesaMensal>();
            IReadOnlyCollection<DespesaMensalView> despesaMensalView = new List<DespesaMensalView>();

            _repositoryMock.Setup(x => x.ObterDespesasMensaisAsync()).ReturnsAsync(despesaMensal);
            _mapperMock.Setup(x => x.Map<IReadOnlyCollection<DespesaMensalView>>(despesaMensal)).Returns(despesaMensalView);

            var viewResult = (await _controller.GetAsync()) as ObjectResult;

            viewResult.StatusCode.Should().Be(404);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalObterDespesaMensalPorIdAsyncOkResult()
        {
            var despesaMensal = new Mock<DespesaMensal>().Object;
            var despesaMensalView = new DespesaMensalView();

            _repositoryMock.Setup(x => x.ObterDespesaMensalPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(despesaMensal);
            _mapperMock.Setup(x => x.Map<DespesaMensalView>(despesaMensal)).Returns(despesaMensalView);

            var viewResult = (await _controller.GetByIdAsync(It.IsAny<Guid>())) as ObjectResult;
            
            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalObterDespesaMensalPorIdAsyncNotFoundResult()
        {
            var despesaMensal = new Mock<DespesaMensal>().Object;

            _repositoryMock.Setup(x => x.ObterDespesaMensalPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(despesaMensal);
            _mapperMock.Setup(x => x.Map<DespesaMensalView>(despesaMensal)).Returns(It.IsAny<DespesaMensalView>());

            var viewResult = (await _controller.GetByIdAsync(It.IsAny<Guid>())) as ObjectResult;
            
            viewResult.StatusCode.Should().Be(404);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalNaoAdicionarAsyncBadRequest()
        {
            var commandModel = new AddDespesaMensalCommandView();
            var command = new AddDespesaMensalCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<AddDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(false);

            var viewResult = (await _controller.PostAsync(commandModel)) as ObjectResult;
            
            viewResult.StatusCode.Should().Be(400);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalAdicionarAsyncOkResult()
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

            var viewResult = (await _controller.PostAsync(commandModel)) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalNaoAtualizarAsyncBadRequest()
        {
            var commandModel = new EditDespesaMensalCommandView();
            var command = new EditDespesaMensalCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<EditDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(false);

            var viewResult = (await _controller.PutAsync(commandModel)) as ObjectResult;

            viewResult.StatusCode.Should().Be(400);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalAtualizarAsyncOkResult()
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

            var viewResult = (await _controller.PutAsync(commandModel)) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalNaoDeletarAsyncBadRequestResult()
        {
            var commandModel = new DeleteDespesaMensalCommandView();
            var command = new DeleteDespesaMensalCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<DeleteDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(false);

            var viewResult = (await _controller.DeleteAsync(commandModel)) as ObjectResult;

            viewResult.StatusCode.Should().Be(400);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DespesaMensalDeletarAsyncOkResult()
        {
            var commandModel = new DeleteDespesaMensalCommandView
            {
                Id = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                IdSalario = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Status = false
            };
            var command = new DeleteDespesaMensalCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(false);
            _mapperMock.Setup(x => x.Map<DeleteDespesaMensalCommand>(commandModel)).Returns(command);
            _mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(true);

            var viewResult = (await _controller.DeleteAsync(commandModel)) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }
    }
}
