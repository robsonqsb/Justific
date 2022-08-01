using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioMembro : RepositorioBase<Membro>, IRepositorioMembro
    {
        public RepositorioMembro(IJustificContext justificContext)
            : base(justificContext)
        {
        }
        public async Task Excluir(int id)
        {
            var query = "call p_excluir_membro(@id);";

            await justificContext
                .Conexao.ExecuteAsync(query, new { id });
        }

        public async Task<IEnumerable<ItemListaMembroOrganizacaoDto>> Listar()
        {
            return await ObterRegistros();
        }

        public async Task<Membro> Obter(string codigoRegistro, int organizacaoId)
        {
            var registros = await ObterRegistros(codigoRegistro, organizacaoId);
            var registro = registros.SingleOrDefault();

            return registro != null ? new Membro()
            {
                Id = registro.MembroId,
                CodigoRegistro = registro.CodigoRegistro,
                Nome = registro.NomeMembro,
                DataCriacao = registro.DataCriacaoMembro,
                AlteradoEm = registro.MembroAlteradoEm,
                Organizacao = new Organizacao()
                {
                    Id = registro.OrganizacaoId,
                    Cnpj = registro.Cnpj,
                    Nome = registro.NomeOrganizacao
                }
            } : null;
        }

        public async Task<int> Salvar(string codigoRegistro, string nome, string cnpjOrganizacao)
        {
            var query = "select f_incluir_alterar_membro(@codigoRegistro, @nome, @cnpjOrganizacao);";

            var id = await justificContext
                .Conexao.ExecuteScalarAsync<int>(query, new { codigoRegistro, nome, cnpjOrganizacao, });

            return id;
        }

        private async Task<IEnumerable<ItemListaMembroOrganizacaoDto>> ObterRegistros(string codigoRegistro = null, int? organizacoaId = null)
        {
            var query = @$"select membro_id,
                                  codigo_registro,
                                  nome_membro,
                                  data_criacao_membro,
                                  membro_alterado_em,
                                  organizacao_id,
                                  nome_organizacao,
                                  cnpj
                            from vw_listar_membros
                           where (@codigoRegistro is null or (@codigoRegistro is not null and codigo_registro = @codigoRegistro)) and
                                 (@organizacoaId is null or (@organizacoaId is not null and organizacao_id = @organizacoaId));";

            return await justificContext
                .Conexao.QueryAsync<ItemListaMembroOrganizacaoDto>(query, new { codigoRegistro, organizacoaId });
        }
    }
}
