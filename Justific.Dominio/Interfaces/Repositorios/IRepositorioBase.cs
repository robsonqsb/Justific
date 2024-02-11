using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioBase<T> where T : EntidadeBase
    {
        Task Excluir(string query, object param);
        Task<IEnumerable<TDto>> Listar<TDto>(string query) where TDto : BaseDto;
        Task<TDto> Obter<TDto>(string query, object param) where TDto : BaseDto;
        Task<long> Salvar(string query, object param);
    }
}
