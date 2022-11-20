using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioJustificativa : RepositorioBase<Justificativa>, IRepositorioJustificativa
    {
        private readonly string camposViewListarJustificativas;

        public RepositorioJustificativa(IJustificContext justificContext)
            : base(justificContext)
        {
            camposViewListarJustificativas = @"justificativa_id JustificativaId,
                                               data_ocorrencia DataOcorrencia,
                                               possui_comprovante PossuiComprovante,
                                               comentarios,
                                               data_criacao DataCriacao,
                                               alterado_em AlteradoEm,
                                               membro_id MembroId,
                                               codigoregistro,
                                               nomemembro,
                                               organizacaoid,
                                               nome_organizacao NomeOrganizacao,
                                               cnpj";
        }

        public async Task Excluir(int justificativaId)
        {
            var query = justificContext.Conexao is NpgsqlConnection ?
                "call p_excluir_justificativa(@justificativaId);" :
                "EXEC SP_EXCLUIR_JUSTIFICATIVA @justificativaId";

            await justificContext
                .Conexao.ExecuteAsync(query, new { justificativaId });
        }

        public async Task<IEnumerable<ItemListaJustificativaDto>> Listar()
        {
            var query = @$"select {camposViewListarJustificativas} from vw_listar_justificativas;";

            return await justificContext
                .Conexao.QueryAsync<ItemListaJustificativaDto>(query);
        }

        public async Task<Justificativa> Obter(string codigoRegistroMembro, string cnpjOrganizacao, DateTime? dataOcorrencia)
        {
            var query = justificContext.Conexao is NpgsqlConnection ?
                $"select {camposViewListarJustificativas} from f_obter_justificativa(@codigoRegistroMembro, @cnpjOrganizacao, @dataOcorrencia::date);" :
                "EXEC SP_OBTER_JUSTIFICATIVA @codigoRegistroMembro, @cnpjOrganizacao, @dataOcorrencia";

            var resultado = await justificContext
                .Conexao.QueryFirstOrDefaultAsync<ItemListaJustificativaDto>(query, new { codigoRegistroMembro, cnpjOrganizacao, dataOcorrencia });

            return resultado != null ? new Justificativa()
            {
                Id = resultado.JustificativaId,
                DataOcorrencia = resultado.DataOcorrencia,
                PossuiComprovante = resultado.PossuiComprovante,
                Comentarios = resultado.Comentarios,
                DataCriacao = resultado.DataCriacao,
                AlteradoEm = resultado.AlteradoEm,
                Membro = new Membro()
                {
                    Id = resultado.MembroId,
                    CodigoRegistro = resultado.CodigoRegistro,
                    Nome = resultado.NomeMembro,
                    Organizacao = new Organizacao()
                    {
                        Id = resultado.OrganizacaoId,
                        Nome = resultado.NomeOrganizacao,
                        Cnpj = resultado.CNPJ
                    }
                }
            } : null;
        }

        public async Task<int> Salvar(string codigoRegistroMembro, string cnpjOrganizacao, string comentarios, DateTime? dataOcorrencia, bool? possuiComprovante)
        {
            var query = justificContext.Conexao is NpgsqlConnection ?
                "select f_incluir_alterar_justificativa(@codigoRegistroMembro, @cnpjOrganizacao, @comentarios, @dataOcorrencia::date, @possuiComprovante::boolean);" :
                "EXEC SP_INCLUIR_ALTERAR_JUSTIFICATIVA @codigoRegistroMembro, @cnpjOrganizacao, @comentarios, @dataOcorrencia, @possuiComprovante";

            return await justificContext
                .Conexao.ExecuteScalarAsync<int>(query, new { codigoRegistroMembro, cnpjOrganizacao, comentarios, dataOcorrencia, possuiComprovante });
        }
    }
}
