using System;

namespace WebApi.DomainEvents.Models
{
    public class EditSalarioCommandView : AddSalarioCommandView
    {
        public Guid Id { get; set; }

    }
}
