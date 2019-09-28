using System;

namespace WebApi.DomainEvents.Models
{
    public class DeleteSalarioCommandView
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }
    }
}
