using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public interface IRepositorioMembro : IRepositorioBase<Membro>
    {
        Task<IEnumerable<ItemListaMembroOrganizacaoDto>> Listar();
        Task<int> Salvar(string codigoRegistro, string nome, string cnpjOrganizacao);
        Task Excluir(int id);
        Task<Membro> Obter(string codigoRegistro, int organizacaoId);
    }
}
