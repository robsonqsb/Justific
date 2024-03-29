﻿using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioOrganizacao : IRepositorioBase<Organizacao>
    {
        Task<IEnumerable<Organizacao>> Listar();
        Task<long> Salvar(string cnpj, string nome);
        Task Excluir(long id);
        Task<Organizacao> Obter(string cnpj);
        Task<bool> VincularUsuario(string login, string cnpjOrganizacao, bool desfazerVinculo);
        Task<IEnumerable<ItemListaOrganizacaoUsuarioDto>> ListarUsuariosAtrelados(string cnpjOrganizacao);
    }
}
