using AutoMapper;
using YoungGuns.Shared;

namespace YoungGuns.WebApi.Map
{
    public static class AutomapperConfig
    {
        public static IMapper Create()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostTaxSystemRequest, TaxSystem>()
                    .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.taxsystem_name))
                    .ForMember(dest => dest.Fields, opts => opts.MapFrom(src => src.taxsystem_fields));
            });

            return config.CreateMapper();
        }
    }
}