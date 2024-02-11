using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioMembro : IRepositorioBase<Membro>
    {
        Task<Membro> Obter(string codigoRegistro, int organizacaoId);
        Task<int> Salvar(MembroInclusaoDto membro);
        Task<IEnumerable<ItemListaMembroOrganizacaoDto>> Listar();
        Task Excluir(long id);
    }
}
