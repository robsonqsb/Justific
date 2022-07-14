using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public interface IRepositorioFuncionario : IRepositorioBase<Funcionario>
    {
        Task<IEnumerable<Funcionario>> Listar();
        Task<int> Salvar(string codigoRegistro, string nome, string cnpjOrganizacao);
        Task Excluir(int id);
        Task<Funcionario> Obter(string codigoRegistro, int organizacaoId);
    }
}
