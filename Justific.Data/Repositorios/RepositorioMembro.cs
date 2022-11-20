using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using Npgsql;
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
            var query = justificContext.Conexao is NpgsqlConnection ?
                "call p_excluir_membro(@id);" :
                "EXEC SP_EXCLUIR_MEMBRO @id";

            await justificContext
                .Conexao.ExecuteAsync(query, new { id });
        }

        public async Task<IEnumerable<ItemListaMembroOrganizacaoDto>> Listar()
        {
            var query = "select * from vw_listar_membros;";

            return await justificContext
                .Conexao.QueryAsync<ItemListaMembroOrganizacaoDto>(query);
        }

        public async Task<Membro> Obter(string codigoRegistro, int organizacaoId)
        {
            var query = justificContext.Conexao is NpgsqlConnection ?
                "select * from f_obter_membro(@codigoRegistro, @organizacaoId);" :
                "EXEC SP_OBTER_MEMBRO @codigoRegistro, @organizacaoId";

            var resultado = await justificContext
                .Conexao.QuerySingleOrDefaultAsync<ItemListaMembroOrganizacaoDto>(query, new { codigoRegistro, organizacaoId });

            return resultado != null ? new Membro()
            {
                Id = resultado.MembroId,
                CodigoRegistro = resultado.CodigoRegistro,
                Nome = resultado.NomeMembro,
                DataCriacao = resultado.DataCriacaoMembro,
                AlteradoEm = resultado.MembroAlteradoEm,
                Organizacao = new Organizacao()
                {
                    Id = resultado.OrganizacaoId,
                    Nome = resultado.NomeOrganizacao,
                    Cnpj = resultado.Cnpj
                }
            } : null;
        }

        public async Task<int> Salvar(string codigoRegistro, string nome, string cnpjOrganizacao)
        {
            var query = justificContext.Conexao is NpgsqlConnection ? 
                "select f_incluir_alterar_membro(@codigoRegistro, @nome, @cnpjOrganizacao);" :
                "EXEC SP_INCLUIR_ALTERAR_MEMBRO @codigoRegistro, @nome, @cnpjOrganizacao";

            var id = await justificContext
                .Conexao.ExecuteScalarAsync<int>(query, new { codigoRegistro, nome, cnpjOrganizacao, });

            return id;
        }
    }
}
