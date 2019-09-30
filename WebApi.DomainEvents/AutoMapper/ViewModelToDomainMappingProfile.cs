using AutoMapper;
using WebApi.DomainEvents.Models;
using WebAppDomainEvents.Domain.Commands.SalarioCommand;

namespace WebApi.DomainEvents.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<AddSalarioCommandView, AddSalarioCommand>();
            CreateMap<EditSalarioCommandView, EditSalarioCommand>();
            CreateMap<DeleteSalarioCommandView, DeleteSalarioCommand>();
        }
    }
}
