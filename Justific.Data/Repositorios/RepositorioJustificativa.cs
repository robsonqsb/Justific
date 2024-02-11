using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioJustificativa : RepositorioBaseRelacional<Justificativa>, IRepositorioJustificativa
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

        public async Task Excluir(long justificativaId)
        {
            await base.Excluir("call p_excluir_justificativa(@justificativaId);", new { justificativaId });
        }

        public async Task<IEnumerable<ItemListaJustificativaDto>> Listar()
        {
            return await base.Listar<ItemListaJustificativaDto>(@$"select {camposViewListarJustificativas} from vw_listar_justificativas;");
        }

        public async Task<ItemListaJustificativaDto> Obter(string codigoRegistroMembro, string cnpjOrganizacao, DateTime? dataOcorrencia)
        {
            var query = $"select {camposViewListarJustificativas} from f_obter_justificativa(@codigoRegistroMembro, @cnpjOrganizacao, @dataOcorrencia::date);";
            return await base.Obter<ItemListaJustificativaDto>(query, new { codigoRegistroMembro, cnpjOrganizacao, dataOcorrencia });
        }

        public async Task<long> Salvar(JustificativaInclusaoDto justificativaInclusaoDto)
        {
            var query = "select f_incluir_alterar_justificativa(@codigoRegistroMembro, @cnpjOrganizacao, @comentarios, @dataOcorrencia::date, @possuiComprovante::boolean);";

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
