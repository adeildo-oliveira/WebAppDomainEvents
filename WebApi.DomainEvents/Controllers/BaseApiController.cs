using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        protected BaseApiController(INotificationHandler<DomainNotification> notifications) => _notifications = (DomainNotificationHandler)notifications;

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation() => !_notifications.HasNotifications();

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    resultado = result,
                    StatusCode = (int)HttpStatusCode.OK
                });
            }

            return BadRequest(new ResponseMensage
            {
                Mensagem = _notifications.GetNotifications().Select(n => n.Value),
                StatusCode = (int)HttpStatusCode.BadRequest
            });
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
        public object Mensagem { get; set; }
        public int StatusCode { get; set; }
    }
}