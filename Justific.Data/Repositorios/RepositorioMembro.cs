﻿using Amazon.Runtime.Internal;
using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using Npgsql;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioMembro : RepositorioBaseRelacional<Membro>, IRepositorioMembro
    {
        public RepositorioMembro(IJustificContext justificContext)
            : base(justificContext)
        {
        }

        public async Task Excluir(long id)
        {
            await justificContext
                .Conexao.ExecuteAsync("call p_excluir_membro(@id);", new { id });
        }

        public async Task<IEnumerable<ItemListaMembroOrganizacaoDto>> Listar()
        {
            return await base.Listar<ItemListaMembroOrganizacaoDto>("select * from vw_listar_membros;");
        }

        public async Task<Membro> Obter(string codigoRegistro, int organizacaoId)
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

            return await base.Obter(sqlQuery.ToString(), new { codigoRegistro, organizacaoId });
        }

        public async Task<int> Salvar(MembroInclusaoDto membro)
        {
            var query = "select f_incluir_alterar_membro(@codigoRegistro, @nome, @cnpjOrganizacao);";

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
