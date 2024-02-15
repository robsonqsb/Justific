using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios.SQL_Server
{
    public class RepositorioMembro : RepositorioBase<Membro>, IRepositorioMembro
    {
        public RepositorioMembro(IJustificContext justificContext) : base(justificContext)
        {
        }

        public async Task Excluir(long id)
        {
            await justificContext
                .Conexao.ExecuteAsync("EXEC SP_EXCLUIR_MEMBRO @id", new { id });
        }

        public async Task<IEnumerable<ItemListaMembroOrganizacaoDto>> Listar()
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
            sqlQuery.AppendLine("   FROM VW_LISTAR_MEMBROS");

            return await justificContext
                .Conexao.QueryAsync<ItemListaMembroOrganizacaoDto>(sqlQuery.ToString());
        }

        public async Task<ItemListaMembroOrganizacaoDto> Obter(string codigoRegistro, int organizacaoId)
        {
            return await base.Obter<ItemListaMembroOrganizacaoDto>("EXEC SP_OBTER_MEMBRO @codigoRegistro, @organizacaoId", new { codigoRegistro, organizacaoId });
        }

        public async Task<long> Salvar(MembroInclusaoDto membro)
        {
            var query = "EXEC SP_INCLUIR_ALTERAR_MEMBRO @codigoRegistro, @nome, @cnpjOrganizacao";

            return await justificContext
                .Conexao.ExecuteScalarAsync<int>(query, new
                {
                    codigoRegistro = membro.CodigoRegistro,
                    nome = membro.Nome,
                    cnpjOrganizacao = membro.CnpjOrganizacao
                });
        }
    }
}
