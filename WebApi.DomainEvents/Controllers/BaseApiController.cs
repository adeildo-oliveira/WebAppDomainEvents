using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using WebAppDomainEvents.Domain.Notifications;

namespace WebApi.DomainEvents.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        protected readonly ILogger _logger;

        protected BaseApiController(INotificationHandler<DomainNotification> notifications, ILogger logger)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _logger = logger;
        }

        protected IEnumerable<DomainNotification> Notifications
        {
            get
            {
                return _notifications.GetNotifications();
            }
        }

        protected bool IsValidOperation() => !_notifications.HasNotifications();

        protected new IActionResult Response(object result = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (IsValidOperation())
            {
                _logger.Information($"RESULTADO :: {JsonConvert.SerializeObject(result)}");
                return StatusCode((int)statusCode, new
                {
                    result,
                    StatusCode = (int)statusCode
                });
            }

            var responseMensage = new ResponseMensage
            {
                Mensagens = _notifications.GetNotifications(),
                StatusCode = (int)statusCode
            };

            _logger.Information($"VALIDAÇÕES :: {JsonConvert.SerializeObject(responseMensage)}");
            return StatusCode((int)statusCode, responseMensage);
        }

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message) => _notifications.Handle(new DomainNotification(code, message), new CancellationToken());
    }

    partial class ResponseMensage
    {
        public object Mensagens { get; set; }
        public int StatusCode { get; set; }
    }
}