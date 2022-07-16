using Justific.Data;
using Justific.Data.Repositorios;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Justific.IoC
{
    public class Registrador
    {
        private readonly IServiceCollection services;

        public Registrador(IServiceCollection services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void Registrar()
        {
            services.TryAddScoped<IJustificContext, JustificContext>();
            services.TryAddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.TryAddScoped<IRepositorioOrganizacao, RepositorioOrganizacao>();
            services.TryAddScoped<IRepositorioMembro, RepositorioMembro>();
        }
    }
}
