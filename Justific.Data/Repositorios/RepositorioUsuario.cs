﻿using Dapper;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(IJustificContext justificContext)
            : base(justificContext)
        {
        }

        public async Task<bool> ConfirmarLogin(string login, string senha)
        {
            var query = justificContext.Conexao is NpgsqlConnection ? 
                "select f_confirmar_login_usuario(@login, @senha);" :
                "EXEC SP_CONFIRMAR_LOGIN_USUARIO @login, @senha";

            var confirmado = await justificContext
                .Conexao.ExecuteScalarAsync<bool>(query, new { login, senha });

            return confirmado;
        }

        public async Task Excluir(long id)
        {
            var query = justificContext.Conexao is NpgsqlConnection ?
                        "call p_excluir_usuario(@id);" :
                        "EXEC SP_EXCLUIR_USUARIO @id";

            await justificContext
                .Conexao.ExecuteAsync(query, new { id });
        }

        public async Task<long> IncluirAlterar(string login, string senha)
        {
            var query = justificContext.Conexao is NpgsqlConnection ?
                "select f_incluir_alterar_usuario(@login, @senha);" :
                "EXEC SP_INCLUIR_ALTERAR_USUARIO @login, @senha";

            var id = await justificContext
                .Conexao.ExecuteScalarAsync<long>(query, new { login, senha });

            return id;
        }

        public async Task<IEnumerable<Usuario>> Listar()
        {
            var query = @"select id,
                                 login,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm
                            from vw_listar_usuarios;";

            return await justificContext
                .Conexao.QueryAsync<Usuario>(query);
        }

        public async Task<Usuario> Obter(string login)
        {
            var query = justificContext.Conexao is NpgsqlConnection ?
                        @"select id,
                                 login,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm
                            from f_obter_usuario (@login);" :
                        "EXEC SP_OBTER_USUARIO @login";

            return await justificContext
                .Conexao.QueryFirstOrDefaultAsync<Usuario>(query, new { login });
        }
    }
}
