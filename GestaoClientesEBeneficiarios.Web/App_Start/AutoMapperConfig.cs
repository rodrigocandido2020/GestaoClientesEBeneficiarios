using AutoMapper;
using System;
using System.Linq;
using System.Reflection;
using GestaoClientesEBeneficiarios.Domain.Entidades;
using GestaoClientesEBeneficiarios.Web.ViewModels;

namespace GestaoClientesEBeneficiarios.Web
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration GetMapperConfiguration()
        {
            var profiles = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x =>
                    typeof(Profile).IsAssignableFrom(x) &&
                    !x.IsAbstract &&
                    x.GetConstructor(Type.EmptyTypes) != null)
                .Select(t => (Profile)Activator.CreateInstance(t))
                .ToList();

            return new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

        }
    }

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Cliente, ClienteViewModel>().ReverseMap();
        }
    }
}
