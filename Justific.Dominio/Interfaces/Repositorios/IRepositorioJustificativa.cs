using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioJustificativa : IRepositorioBase<Justificativa>
    {
        Task<int> Salvar(string codigoRegistroMembro, string cnpjOrganizacao, string comentarios, DateTime? dataOcorrencia, bool? possuiComprovante);
        Task Excluir(int justificativaId);
        Task<IEnumerable<ItemListaJustificativaDto>> Listar();
        Task<Justificativa> Obter(string codigoRegistroMembro, string cnpjOrganizacao, DateTime? dataOcorrencia);
    }
}
