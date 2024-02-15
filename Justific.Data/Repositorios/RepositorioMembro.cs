using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioMembro : RepositorioBase<Membro>, IRepositorioMembro
    {
        public RepositorioMembro(IJustificContext justificContext)
            : base(justificContext)
        {
        }

        public async Task Excluir(long id)
        {
            await base.Excluir("call p_excluir_membro(@id);", new { id });
        }

        public async Task<IEnumerable<ItemListaMembroOrganizacaoDto>> Listar()
        {
            return await base.Listar<ItemListaMembroOrganizacaoDto>("select * from vw_listar_membros;");
        }

        public async Task<ItemListaMembroOrganizacaoDto> Obter(string codigoRegistro, int organizacaoId)
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.AppendLine("SELECT MembroId,");
            sqlQuery.AppendLine("       CodigoRegistro,");
            sqlQuery.AppendLine("       NomeMembro,");
            sqlQuery.AppendLine("       DataCriacaoMembro,");
            sqlQuery.AppendLine("       MembroAlteradoEm,");
            sqlQuery.AppendLine("       OrganizacaoId,");
            sqlQuery.AppendLine("       NomeOrganizacao,");
            sqlQuery.AppendLine("       CNPJ");
            sqlQuery.AppendLine("   from f_obter_membro(@codigoRegistro, @organizacaoId);");

            return await base.Obter<ItemListaMembroOrganizacaoDto>(sqlQuery.ToString(), new { codigoRegistro, organizacaoId });
        }

        public async Task<long> Salvar(MembroInclusaoDto membro)
        {
            var query = "select f_incluir_alterar_membro(@codigoRegistro, @nome, @cnpjOrganizacao);";

            return await base.Salvar(query, new
            {
                codigoRegistro = membro.CodigoRegistro,
                nome = membro.Nome,
                cnpjOrganizacao = membro.CnpjOrganizacao
            });
        }
    }
}
