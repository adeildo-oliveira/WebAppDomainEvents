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
using WebApi.DomainEvents.Models.CommandsView.SalarioCommandView;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;
using WebAppDomainEvents.Domain.Interfaces.Repository;
using WebAppDomainEvents.Domain.Models;
using WebAppDomainEvents.Domain.Notifications;
using Xunit;

namespace Tests.Unit.WebApi.v1
{
    public class SalarioControllerTests
    {
        private readonly AutoMocker _mocker;
        private readonly SalarioController _controller;
        private readonly Mock<DomainNotificationHandler> _notificationMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ISalarioRepositoryReadOnly> _repositoryMock;
        private readonly Mock<ILogger> _loggerMock;

        public SalarioControllerTests()
        {
            _mocker = new AutoMocker();
            _notificationMock = _mocker.GetMock<DomainNotificationHandler>();
            _mediatorMock = _mocker.GetMock<IMediator>();
            _mapperMock = _mocker.GetMock<IMapper>();
            _repositoryMock = _mocker.GetMock<ISalarioRepositoryReadOnly>();
            _loggerMock = _mocker.GetMock<ILogger>();

            _controller = new SalarioController(_notificationMock.Object,
                _loggerMock.Object,
                _mediatorMock.Object,
                _mapperMock.Object,
                _repositoryMock.Object);
        }

        [Fact]
        public async Task ObterSalarioNotResult()
        {
            var salarios = new Mock<IReadOnlyCollection<Salario>>().Object;
            IReadOnlyCollection<SalarioView> salariosView = new List<SalarioView>();

            _repositoryMock.Setup(x => x.ObterSalariosAsync()).ReturnsAsync(salarios);
            _mapperMock.Setup(x => x.Map<IReadOnlyCollection<SalarioView>>(salarios)).Returns(salariosView);
            
            var viewResult = (await _controller.GetAsync()) as ObjectResult;

            viewResult.StatusCode.Should().Be(404);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task ObterSalarioOkResult()
        {
            var salarios = new Mock<IReadOnlyCollection<Salario>>().Object;
            IReadOnlyCollection<SalarioView> salariosView = new List<SalarioView> { new SalarioView() };

            _repositoryMock.Setup(x => x.ObterSalariosAsync()).ReturnsAsync(salarios);
            _mapperMock.Setup(x => x.Map<IReadOnlyCollection<SalarioView>>(salarios)).Returns(salariosView);
            
            var viewResult = (await _controller.GetAsync()) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task ObterSalarioPorIdNotFound()
        {
            var salario = new Mock<Salario>().Object;

            _repositoryMock.Setup(x => x.ObterSalarioPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(salario);
            _mapperMock.Setup(x => x.Map<SalarioView>(salario)).Returns(It.IsAny<SalarioView>());

            var viewResult = (await _controller.GetByIdAsync(It.IsAny<Guid>())) as ObjectResult;

            viewResult.StatusCode.Should().Be(404);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task ObterSalarioPorIdOkResult()
        {
            var salario = new Mock<Salario>().Object;
            var salarioView = new SalarioView();

            _repositoryMock.Setup(x => x.ObterSalarioPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(salario);
            _mapperMock.Setup(x => x.Map<SalarioView>(salario)).Returns(salarioView);

            var viewResult = (await _controller.GetByIdAsync(It.IsAny<Guid>())) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task PostDeveAdicionarSalarioOkResult()
        {
            var model = new AddSalarioCommandView { Adiantamento = decimal.One, Pagamento = decimal.One};
            var addSalarioCommand = new AddSalarioCommand { Adiantamento = decimal.One, Pagamento = decimal.One };

            _notificationMock.Setup(x => x.HasNotifications()).Returns(false);
            _mapperMock.Setup(x => x.Map<AddSalarioCommand>(model)).Returns(addSalarioCommand);
            _mediatorMock.Setup(x => x.Send(addSalarioCommand, default)).ReturnsAsync(true);

            var viewResult = (await _controller.PostAsync(model)) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task PostNaoDeveAdicionarSalarioBadRequest()
        {
            var model = new AddSalarioCommandView();
            var addSalarioCommand = new AddSalarioCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<AddSalarioCommand>(model)).Returns(addSalarioCommand);
            _mediatorMock.Setup(x => x.Send(addSalarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.PostAsync(model)) as ObjectResult;

            viewResult.StatusCode.Should().Be(400);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task PutDeveEditarSalarioOkResult()
        {
            var model = new EditSalarioCommandView 
            {
                Id = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Adiantamento = decimal.One, 
                Pagamento = decimal.One 
            };
            var salarioCommand = new EditSalarioCommand
            {
                Id = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Adiantamento = decimal.One,
                Pagamento = decimal.One
            };

            _notificationMock.Setup(x => x.HasNotifications()).Returns(false);
            _mapperMock.Setup(x => x.Map<EditSalarioCommand>(model)).Returns(salarioCommand);
            _mediatorMock.Setup(x => x.Send(salarioCommand, default)).ReturnsAsync(true);

            var viewResult = (await _controller.PutAsync(model)) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task PutNaoDeveEditarSalarioBadRequest()
        {
            var model = new EditSalarioCommandView();
            var salarioCommand = new EditSalarioCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<EditSalarioCommand>(model)).Returns(salarioCommand);
            _mediatorMock.Setup(x => x.Send(salarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.PutAsync(model)) as ObjectResult;

            viewResult.StatusCode.Should().Be(400);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DeleteDeveExcluirSalarioOkResult()
        {
            var model = new DeleteSalarioCommandView
            {
                Id = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Status = false
            };
            var salarioCommand = new DeleteSalarioCommand
            {
                Id = new Guid("10AFDB5E-D7D1-4773-B040-F7B6F610484F"),
                Status = false
            };

            _notificationMock.Setup(x => x.HasNotifications()).Returns(false);
            _mapperMock.Setup(x => x.Map<DeleteSalarioCommand>(model)).Returns(salarioCommand);
            _mediatorMock.Setup(x => x.Send(salarioCommand, default)).ReturnsAsync(true);

            var viewResult = (await _controller.DeleteAsync(model)) as ObjectResult;

            viewResult.StatusCode.Should().Be(200);
            viewResult.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task DeleteNaoDeveExcluirSalarioBadRequest()
        {
            var model = new DeleteSalarioCommandView();
            var salarioCommand = new DeleteSalarioCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<DeleteSalarioCommand>(model)).Returns(salarioCommand);
            _mediatorMock.Setup(x => x.Send(salarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.DeleteAsync(model)) as ObjectResult;

            viewResult.StatusCode.Should().Be(400);
            viewResult.Should().BeOfType<ObjectResult>();
        }
    }
}
