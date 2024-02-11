using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioBaseNoSql<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        protected readonly IJustificContext justificContext;

        public RepositorioBaseNoSql(IJustificContext justificContext)
        {
            this.justificContext = justificContext ?? throw new ArgumentNullException(nameof(justificContext));
        }

        public Task Excluir(string query, object param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TDto>> Listar<TDto>(string query) where TDto : BaseDto
        {
            throw new NotImplementedException();
        }

        public Task<T> Obter(string query, object param)
        {
            throw new NotImplementedException();
        }

        public Task<int> Salvar(string query, object param)
        {
            throw new NotImplementedException();
        }
    }
}
