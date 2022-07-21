using Dapper;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioJustificativa : RepositorioBase<Justificativa>, IRepositorioJustificativa
    {
        public RepositorioJustificativa(IJustificContext justificContext)
            : base(justificContext)
        {
        }

        public async Task Excluir(int justificativaId)
        {
            var query = "call p_excluir_membro(@justificativaId);";

            await justificContext
                .Conexao.ExecuteAsync(query, new { justificativaId });
        }

        public async Task<IEnumerable<ItemListaJustificativaDto>> Listar()
        {
            return await ObterListaItens();
        }

        public async Task<Justificativa> Obter(string codigoRegistroMembro, string cnpjOrganizacao, DateTime? dataOcorrencia)
        {
            var listaRetorno = await ObterListaItens(codigoRegistroMembro, cnpjOrganizacao, dataOcorrencia);
            var registro = listaRetorno.SingleOrDefault();

            return registro != null ? new Justificativa()
            {
                Id = registro.JustificativaId,
                DataOcorrencia = registro.DataOcorrencia,
                PossuiComprovante = registro.PossuiComprovante,
                Comentarios = registro.Comentarios,
                DataCriacao = registro.DataCriacao,
                AlteradoEm = registro.AlteradoEm,
                Membro = new Membro()
                {
                    Id = registro.MembroId,
                    CodigoRegistro = registro.CodigoRegistro,
                    Nome = registro.NomeMembro,
                    Organizacao = new Organizacao()
                    {
                        Id = registro.OrganizacaoId,
                        Nome = registro.NomeOrganizacao,
                        Cnpj = registro.CNPJ
                    }
                }
            } : null;
        }

        public async Task<int> Salvar(string codigoRegistroMembro, string cnpjOrganizacao, string comentarios, DateTime? dataOcorrencia, bool? possuiComprovante)
        {
            var query = "select f_incluir_alterar_justificativa(@codigoRegistroMembro, @cnpjOrganizacao, @comentarios, @dataOcorrencia::date, @possuiComprovante::boolean);";

            var id = await justificContext
                .Conexao.ExecuteScalarAsync<int>(query, new { codigoRegistroMembro, cnpjOrganizacao, comentarios, dataOcorrencia, possuiComprovante });

            return id;
        }

        private async Task<IEnumerable<ItemListaJustificativaDto>> ObterListaItens(string codigoRegistroMembro = null, string cnpjOrganizacao = null, DateTime? dataOcorrencia = null)
        {
            var query = @"select justificativa_id JustificativaId,
                                 data_ocorrencia DataOcorrencia,
                                 possui_comprovante PossuiComprovante,
                                 comentarios,
                                 data_criacao DataCriacao,
                                 alterado_em AlteradoEm,
                                 membro_id MembroId,
                                 codigo_registro CodigoRegistro,
                                 nome_membro NomeMembro,
                                 organizacao_id OrganizacaoId,
                                 nome_organizacao NomeOrganizacao,
                                 cnpj
                            from vw_listar_justificativas
                          where (@codigoRegistroMembro::text is null or (@codigoRegistroMembro::text is not null and codigo_registro = @codigoRegistroMembro::text)) and
                                (@cnpjOrganizacao::text is null or (@cnpjOrganizacao::text is not null and cnpj = @cnpjOrganizacao::text)) and
                                (@dataOcorrencia::date is null or (@dataOcorrencia::date is not null and data_ocorrencia = @dataOcorrencia::date));";

            return await justificContext
                .Conexao.QueryAsync<ItemListaJustificativaDto>(query, new { codigoRegistroMembro, cnpjOrganizacao, dataOcorrencia });
        }
    }
}
