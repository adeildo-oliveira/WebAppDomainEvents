using AutoMapper;
using WebApi.DomainEvents.Models;
using WebAppDomainEvents.Domain.Models;

namespace WebApi.DomainEvents.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<DespesaMensal, DespesaMensalView>();
            CreateMap<Salario, SalarioView>();

            CreateMap<DespesaMensal, DespesaMensalSalarioView>();
            CreateMap<Salario, DespesaSalarioView>();

            CreateMap<Salario, SalarioView>()
                .ForMember(d => d.DespesasMensais, opt => opt.MapFrom(src => src.DespesasMensais));
            CreateMap<DespesaMensal, DespesaMensalView>()
                .ForMember(d => d.Salario, opt => opt.MapFrom(src => src.Salario));
        }
    }
}
