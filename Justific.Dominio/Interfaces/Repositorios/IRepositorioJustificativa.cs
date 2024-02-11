using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioJustificativa : IRepositorioBase<Justificativa>
    {
        Task<IEnumerable<ItemListaJustificativaDto>> Listar();

        Task<long> Salvar(JustificativaInclusaoDto justificativaInclusaoDto);

        Task Excluir(long justificativaId);

        Task<Justificativa> Obter(string codigoRegistroMembro, string cnpjOrganizacao, DateTime? dataOcorrencia);
    }
}
