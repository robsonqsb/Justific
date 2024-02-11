using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioBase<T> where T : EntidadeBase
    {
        Task Excluir(string query, object param);
        Task<IEnumerable<TDto>> Listar<TDto>(string query) where TDto : BaseDto;
        Task<T> Obter(string query, object param);
        Task<int> Salvar(string query, object param);
    }
}
