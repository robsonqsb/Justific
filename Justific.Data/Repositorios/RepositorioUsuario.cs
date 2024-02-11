using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioUsuario : RepositorioBaseRelacional<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(IJustificContext justificContext)
            : base(justificContext)
        {
        }

        public async Task<bool> ConfirmarLogin(string login, string senha)
        {
            return await justificContext
                .Conexao.ExecuteScalarAsync<bool>("select f_confirmar_login_usuario(@login, @senha);", new { login, senha });
        }

        public async Task Excluir(long id)
        {
            await base.Excluir("call p_excluir_usuario(@id);", new { id });
        }

        public async Task<long> IncluirAlterar(string login, string senha)
        {
            return await justificContext
                .Conexao.ExecuteScalarAsync<long>("select f_incluir_alterar_usuario(@login, @senha);", new { login, senha });
        }

        public async Task<IEnumerable<UsuarioDto>> Listar()
        {
            var query = @"select id,
                                 login,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm
                            from vw_listar_usuarios;";

            return await base.Listar<UsuarioDto>(query);
        }

        public async Task<UsuarioDto> Obter(string login)
        {
            var query = @"select id,
                                 login,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm
                            from f_obter_usuario (@login);";

            return await Obter<UsuarioDto>(query, new { login });
        }
    }
}
