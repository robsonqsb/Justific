using Justific.Data;
using Justific.Data.Repositorios;
using Justific.Dominio.Enumeradores;
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

        public void Registrar(TipoConexaoBD tipoConexaoDB)
        {
            services.TryAddScoped<IJustificContext, JustificContext>();

            switch (tipoConexaoDB)
            {
                case TipoConexaoBD.SQL_Server:
                    RegistrarSqlServer();
                    break;
                case TipoConexaoBD.MongoDB:
                    break;
                case TipoConexaoBD.PostgreSQL:
                default:
                    RegistrarPostgreSql();
                    break;
            }
        }

        private void RegistrarPostgreSql()
        {
            services.TryAddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.TryAddScoped<IRepositorioOrganizacao, RepositorioOrganizacao>();
            services.TryAddScoped<IRepositorioMembro, RepositorioMembro>();
            services.TryAddScoped<IRepositorioJustificativa, RepositorioJustificativa>();
        }

        private void RegistrarSqlServer()
        {
            services.TryAddScoped<IRepositorioUsuario, Data.Repositorios.SQL_Server.RepositorioUsuario>();
            services.TryAddScoped<IRepositorioOrganizacao, Data.Repositorios.SQL_Server.RepositorioOrganizacao>();
            services.TryAddScoped<IRepositorioMembro, Data.Repositorios.SQL_Server.RepositorioMembro>();
            services.TryAddScoped<IRepositorioJustificativa, Data.Repositorios.SQL_Server.RepositorioJustificativa>();
        }
    }
}
