using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;
using WebAppDomainEvents.Domain.Notifications;

namespace WebApi.DomainEvents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _notifications;

        public ValuesController(IMediator mediator, INotificationHandler<DomainNotification> notificationHandler)
        {
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notificationHandler;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            await _mediator.Send(new AddSalarioCommand { });
            await _mediator.Send(new EditSalarioCommand { });

            foreach (var item in _notifications.GetNotifications())
            {
                var teste = item.Value;
            }

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
