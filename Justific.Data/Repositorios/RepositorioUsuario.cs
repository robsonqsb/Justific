using Dapper;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly IJustificContext justificContext;

        public RepositorioUsuario(IJustificContext justificContext)
        {
            this.justificContext = justificContext ?? throw new ArgumentNullException(nameof(justificContext));
        }

        public async Task<bool> ConfirmarLogin(string login, string senha)
        {
            var sqlQuery = "select f_confirmar_login_usuario(@login, @senha);";

            var confirmado = await justificContext
                .Conexao.ExecuteScalarAsync<bool>(sqlQuery, new { login, senha });

            return confirmado;
        }

        public async Task Excluir(long id)
        {
            var sqlQuery = "call p_excluir_usuario(@id);";

            await justificContext
                .Conexao.ExecuteAsync(sqlQuery, new { id });
        }

        public async Task<long> IncluirAlterar(string login, string senha)
        {
            var sqlQuery = "select f_incluir_alterar_usuario(@login, @senha);";

            var id = await justificContext
                .Conexao.ExecuteScalarAsync<long>(sqlQuery, new { login, senha });

            return id;
        }

        public async Task<IEnumerable<Usuario>> Listar()
        {
            var sqlQuery = @"select id,
                                    login,
                                    data_criacao DataCriacao,
                                    alterado_em AlteradoEm
                                from vw_listar_usuarios;";

            return await justificContext
                .Conexao.QueryAsync<Usuario>(sqlQuery);
        }

        public async Task<Usuario> Obter(string login)
        {
            var sqlQuery = @"select id,
                                    login,
                                    data_criacao DataCriacao,
                                    alterado_em AlteradoEm
                                from f_obter_usuario (@login);";

            return await justificContext
                .Conexao.QueryFirstOrDefaultAsync<Usuario>(sqlQuery, new { login });
        }
    }
}
