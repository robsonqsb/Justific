using Justific.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioUsuario : IRepositorioBase<Usuario>
    {
        Task<IEnumerable<Usuario>> Listar();
        Task<long> IncluirAlterar(string login, string senha);
        Task<Usuario> Obter(string login);
        Task Excluir(long id);
        Task<bool> ConfirmarLogin(string login, string senha);
    }
}
