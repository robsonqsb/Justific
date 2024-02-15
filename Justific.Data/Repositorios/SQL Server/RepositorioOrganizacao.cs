using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios.SQL_Server
{
    public class RepositorioOrganizacao : RepositorioBase<Organizacao>, IRepositorioOrganizacao
    {
        public RepositorioOrganizacao(IJustificContext justificContext) : base(justificContext)
        {
        }

        public async Task Excluir(long id)
        {
            await base.Excluir("EXEC SP_EXCLUIR_ORGANIZACAO @id", new { id });
        }

        public async Task<IEnumerable<OrganizacaoDto>> Listar()
        {
            var query = @"SELECT id,
                                 nome,
                                 cnpj,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm
                            FROM VW_LISTAR_ORGANIZACOES;";

            return await base.Listar<OrganizacaoDto>(query);
        }

        public async Task<IEnumerable<ItemListaOrganizacaoUsuarioDto>> ListarUsuariosAtrelados(string cnpjOrganizacao)
        {
            return await justificContext
                .Conexao.QueryAsync<ItemListaOrganizacaoUsuarioDto>("EXEC SP_LISTAR_ORGANIZACOES_USUARIOS @cnpjOrganizacao", new { cnpjOrganizacao });
        }

        public async Task<OrganizacaoDto> Obter(string cnpj)
        {
            return await base.Obter<OrganizacaoDto>("EXEC SP_OBTER_ORGANIZACAO @cnpj", new { cnpj });
        }

        public async Task<long> Salvar(string cnpj, string nome)
        {
            return await base.Salvar("EXEC SP_INCLUIR_ALTERAR_ORGANIZACAO @nome, @cnpj", new { cnpj, nome });
        }

        public async Task<bool> VincularUsuario(string login, string cnpjOrganizacao, bool desfazerVinculo)
        {
            return await justificContext
                .Conexao.ExecuteScalarAsync<bool>("EXEC SP_VINCULAR_ORGANIZACAO_USUARIO @login, @cnpjOrganizacao, @desfazerVinculo", new { login, cnpjOrganizacao, desfazerVinculo });
        }
    }
}
