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

            CreateMap<Salario, DespesaSalarioView>();

            CreateMap<DespesaMensal, DespesaMensalView>()
                .ForMember(d => d.Salario, opt => opt.MapFrom(src => src.Salario));
        }
    }
}
