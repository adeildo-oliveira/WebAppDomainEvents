using AutoMapper;
using WebApi.DomainEvents.Models;
using WebAppDomainEvents.Domain.Models;

namespace WebApi.DomainEvents.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Salario, SalarioView>()
                .ForMember(d => d.DespesasMensaisView, opt => opt.MapFrom(src => src.DespesasMensais));
            CreateMap<DespesaMensal, DespesaMensalView>()
                .ForMember(d => d.SalarioView, opt => opt.MapFrom(src => src.Salario));
        }
    }
}
