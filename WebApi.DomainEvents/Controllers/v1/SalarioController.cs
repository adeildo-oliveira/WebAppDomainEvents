using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppDomainEvents.Domain.Notifications;

namespace WebApi.DomainEvents.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalarioController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SalarioController(INotificationHandler<DomainNotification> notifications, IMediator mediator) 
            : base(notifications) => _mediator = mediator;

        [HttpGet]
        [Route("ObterSalario/{id:guid}")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
        [HttpGet("ObterSalario/{id:guid}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Salario
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Salario/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
