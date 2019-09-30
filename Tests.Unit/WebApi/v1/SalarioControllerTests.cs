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
        private readonly Mock<ISalarioRepository> _repositoryMock;

        public SalarioControllerTests()
        {
            _mocker = new AutoMocker();
            _notificationMock = _mocker.GetMock<DomainNotificationHandler>();
            _mediatorMock = _mocker.GetMock<IMediator>();
            _mapperMock = _mocker.GetMock<IMapper>();
            _repositoryMock = _mocker.GetMock<ISalarioRepository>();

            _controller = new SalarioController(_notificationMock.Object,
                _mediatorMock.Object,
                _mapperMock.Object,
                _repositoryMock.Object);
        }

        [Fact]
        public async Task ObterSalario()
        {
            var salarios = new Mock<IReadOnlyCollection<Salario>>().Object;
            var salariosView = new Mock<IReadOnlyCollection<SalarioView>>().Object;

            _repositoryMock.Setup(x => x.ObterSalarioAsync()).ReturnsAsync(salarios);
            _mapperMock.Setup(x => x.Map<IReadOnlyCollection<SalarioView>>(salarios)).Returns(salariosView);
            
            var viewResult = (await _controller.ObterSalario()) as OkObjectResult;
            
            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ObterSalarioPorId()
        {
            var salario = new Mock<Salario>().Object;
            var salarioView = new Mock<SalarioView>().Object;

            _repositoryMock.Setup(x => x.ObterSalarioPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(salario);
            _mapperMock.Setup(x => x.Map<SalarioView>(salario)).Returns(salarioView);

            var viewResult = (await _controller.ObterSalarioPorId(It.IsAny<Guid>())) as OkObjectResult;

            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PostDeveAdicionarSalario()
        {
            var model = new AddSalarioCommandView { Adiantamento = decimal.One, Pagamento = decimal.One};
            var addSalarioCommand = new AddSalarioCommand { Adiantamento = decimal.One, Pagamento = decimal.One };

            _notificationMock.Setup(x => x.HasNotifications()).Returns(false);
            _mapperMock.Setup(x => x.Map<AddSalarioCommand>(model)).Returns(addSalarioCommand);
            _mediatorMock.Setup(x => x.Send(addSalarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.PostAdicionar(model)) as OkObjectResult;
            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PostNaoDeveAdicionarSalario()
        {
            var model = new AddSalarioCommandView();
            var addSalarioCommand = new AddSalarioCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<AddSalarioCommand>(model)).Returns(addSalarioCommand);
            _mediatorMock.Setup(x => x.Send(addSalarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.PostAdicionar(model)) as BadRequestObjectResult;
            viewResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PutDeveEditarSalario()
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
            _mediatorMock.Setup(x => x.Send(salarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.PutAdicionar(model)) as OkObjectResult;
            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PutNaoDeveEditarSalario()
        {
            var model = new EditSalarioCommandView();
            var salarioCommand = new EditSalarioCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<EditSalarioCommand>(model)).Returns(salarioCommand);
            _mediatorMock.Setup(x => x.Send(salarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.PutAdicionar(model)) as BadRequestObjectResult;
            viewResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteDeveExcluirSalario()
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
            _mediatorMock.Setup(x => x.Send(salarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.Delete(model)) as OkObjectResult;
            viewResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteNaoDeveExcluirSalario()
        {
            var model = new DeleteSalarioCommandView();
            var salarioCommand = new DeleteSalarioCommand();

            _notificationMock.Setup(x => x.HasNotifications()).Returns(true);
            _mapperMock.Setup(x => x.Map<DeleteSalarioCommand>(model)).Returns(salarioCommand);
            _mediatorMock.Setup(x => x.Send(salarioCommand, default)).ReturnsAsync(false);

            var viewResult = (await _controller.Delete(model)) as BadRequestObjectResult;
            viewResult.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
