using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios.SQL_Server
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(IJustificContext justificContext) : base(justificContext)
        {
        }

        public async Task<bool> ConfirmarLogin(string login, string senha)
        {
            return await justificContext
                .Conexao.ExecuteScalarAsync<bool>("EXEC SP_CONFIRMAR_LOGIN_USUARIO @login, @senha", new { login, senha });
        }

        public async Task Excluir(long id)
        {
            await base.Excluir("EXEC SP_EXCLUIR_USUARIO @id", new { id });
        }

        public async Task<long> IncluirAlterar(string login, string senha)
        {
            return await justificContext
                .Conexao.ExecuteScalarAsync<long>("EXEC SP_INCLUIR_ALTERAR_USUARIO @login, @senha", new { login, senha });
        }

        public async Task<IEnumerable<UsuarioDto>> Listar()
        {
            var query = @"SELECT id,
                                 login,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm
                            FROM VW_LISTAR_USUARIOS";

            return await base.Listar<UsuarioDto>(query);
        }

        public async Task<UsuarioDto> Obter(string login)
        {
            return await Obter<UsuarioDto>("EXEC SP_OBTER_USUARIO @login", new { login });
        }
    }
}
