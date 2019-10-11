using AutoMapper;
using WebApi.DomainEvents.Models.CommandsView.DespesaMensalView;
using WebApi.DomainEvents.Models.CommandsView.SalarioCommandView;
using WebAppDomainEvents.Domain.Commands.DespesaMensalCommand;
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
            
            CreateMap<AddDespesaMensalCommandView, AddDespesaMensalCommand>();
            CreateMap<EditDespesaMensalCommandView, EditDespesaMensalCommand>();
            CreateMap<DeleteDespesaMensalCommandView, DeleteDespesaMensalCommand>();
        }
    }
}
