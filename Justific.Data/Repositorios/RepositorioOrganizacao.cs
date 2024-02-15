using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioOrganizacao : RepositorioBase<Organizacao>, IRepositorioOrganizacao
    {
        public RepositorioOrganizacao(IJustificContext justificContext)
            : base(justificContext)
        {
        }

        public async Task Excluir(long id)
        {
            await base.Excluir("call p_excluir_organizacao(@id);", new { id });
        }

        public async Task<IEnumerable<OrganizacaoDto>> Listar()
        {
            var query = @"select id,
                                 nome,
                                 cnpj,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm
                            from vw_listar_organizacoes;";

            return await base.Listar<OrganizacaoDto>(query);
        }

        public async Task<OrganizacaoDto> Obter(string cnpj)
        {
            var query = @"select id,
                                 nome,
                                 cnpj,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm
                             from f_obter_organizacao (@cnpj);";

            return await base.Obter<OrganizacaoDto>(query, new { cnpj });
        }

        public async Task<long> Salvar(string cnpj, string nome)
        {
            return await base.Salvar("select f_incluir_alterar_organizacao(@nome, @cnpj);", new { cnpj, nome });
        }

        public async Task<bool> VincularUsuario(string login, string cnpjOrganizacao, bool desfazerVinculo)
        {
            return await justificContext
                .Conexao.ExecuteScalarAsync<bool>("select f_vincular_organizacao_usuario(@login, @cnpjOrganizacao, @desfazerVinculo);", new { login, cnpjOrganizacao, desfazerVinculo });
        }

        public async Task<IEnumerable<ItemListaOrganizacaoUsuarioDto>> ListarUsuariosAtrelados(string cnpjOrganizacao)
        {
            var query = @"select organizacao_id OrganizacaoId,
                                 nome_organizacao NomeOrganizacao,
                                 usuario_id UsuarioId,
                                 login_usuario LoginUsuario
                            from f_listar_organizacoes_usuarios(@cnpjOrganizacao);";

            return await justificContext
                .Conexao.QueryAsync<ItemListaOrganizacaoUsuarioDto>(query, new { cnpjOrganizacao });
        }
    }
}
