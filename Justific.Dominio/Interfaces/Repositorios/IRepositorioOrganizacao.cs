using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioOrganizacao : IRepositorioBase<Organizacao>
    {
        Task<bool> VincularUsuario(string login, string cnpjOrganizacao, bool desfazerVinculo);
        Task<IEnumerable<ItemListaOrganizacaoUsuarioDto>> ListarUsuariosAtrelados(string cnpjOrganizacao);
        Task<IEnumerable<OrganizacaoDto>> Listar();
        Task Excluir(long id);
        Task<OrganizacaoDto> Obter(string cnpj);
        Task<long> Salvar(string nome, string cnpj);
    }
}
