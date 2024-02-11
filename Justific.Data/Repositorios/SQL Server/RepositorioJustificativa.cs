using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios.SQL_Server
{
    public class RepositorioJustificativa : RepositorioBaseRelacional<Justificativa>, IRepositorioJustificativa
    {
        private readonly string camposViewListarJustificativas;

        public RepositorioJustificativa(IJustificContext justificContext) : base(justificContext)
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

        public async Task Excluir(long justificativaId)
        {
            await base.Excluir("EXEC SP_EXCLUIR_JUSTIFICATIVA @justificativaId", new { justificativaId });
        }

        public async Task<IEnumerable<ItemListaJustificativaDto>> Listar()
        {
            return await base.Listar<ItemListaJustificativaDto>(@$"SELECT {camposViewListarJustificativas} FROM VW_LISTAR_JUSTIFICATIVAS;");
        }

        public async Task<Justificativa> Obter(string codigoRegistroMembro, string cnpjOrganizacao, DateTime? dataOcorrencia)
        {
            var query = "EXEC SP_OBTER_JUSTIFICATIVA @codigoRegistroMembro, @cnpjOrganizacao, @dataOcorrencia";
            return await base.Obter(query, new { codigoRegistroMembro, cnpjOrganizacao, dataOcorrencia });
        }

        public async Task<long> Salvar(JustificativaInclusaoDto justificativaInclusaoDto)
        {
            var query = "EXEC SP_INCLUIR_ALTERAR_JUSTIFICATIVA @codigoRegistroMembro, @cnpjOrganizacao, @comentarios, @dataOcorrencia, @possuiComprovante";

            return await base.Salvar(query, new
            {
                codigoRegistroMembro = justificativaInclusaoDto.CodigoRegistroMembro,
                cnpjOrganizacao = justificativaInclusaoDto.CnpjOrganizacao,
                comentarios = justificativaInclusaoDto.Comentarios,
                dataOcorrencia = justificativaInclusaoDto.DataOcorrencia,
                possuiComprovante = justificativaInclusaoDto.PossuiComprovante
            });
        }
    }
}
