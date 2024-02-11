using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioMembro : IRepositorioBase<Membro>
    {
        Task<ItemListaMembroOrganizacaoDto> Obter(string codigoRegistro, int organizacaoId);
        Task<long> Salvar(MembroInclusaoDto membro);
        Task<IEnumerable<ItemListaMembroOrganizacaoDto>> Listar();
        Task Excluir(long id);
    }
}
