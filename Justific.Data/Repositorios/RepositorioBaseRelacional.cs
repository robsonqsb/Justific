﻿using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public abstract class RepositorioBaseRelacional<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        protected readonly IJustificContext justificContext;

        public RepositorioBaseRelacional(IJustificContext justificContext)
        {
            this.justificContext = justificContext ?? throw new ArgumentNullException(nameof(justificContext));
        }

        public virtual async Task Excluir(string query, object param)
        {
            await justificContext
                .Conexao.ExecuteAsync(query, param);
        }

        public virtual async Task<IEnumerable<TDto>> Listar<TDto>(string query) where TDto : BaseDto
        {
            return await justificContext
                .Conexao.QueryAsync<TDto>(query);
        }

        public virtual async Task<T> Obter(string query, object param)
        {
            return await justificContext
                .Conexao.QueryFirstOrDefaultAsync<T>(query, param);
        }

        public virtual async Task<int> Salvar(string query, object param)
        {
            return await justificContext
                .Conexao.ExecuteScalarAsync<int>(query, param);
        }
    }
}
