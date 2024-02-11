using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Justific.Api.Controllers
{
    public class OrganizacaoController : BaseController<Organizacao>
    {
        private readonly IRepositorioOrganizacao repositorioOrganizacao;

        public OrganizacaoController(IRepositorioOrganizacao repositorioOrganizacao)
            : base(repositorioOrganizacao)
        {
            this.repositorioOrganizacao = repositorioOrganizacao ?? throw new ArgumentNullException(nameof(repositorioOrganizacao));
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(IEnumerable<OrganizacaoDto>), 200)]
        public async Task<IActionResult> Listar()
        {
            try
            {
                return Ok(await repositorioOrganizacao.Listar());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("salvar")]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<IActionResult> Salvar(string cnpj, string nome)
        {
            try
            {
                return Ok(await repositorioOrganizacao
                    .Salvar(cnpj, nome));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("excluir")]
        public async Task<IActionResult> Excluir(long id)
        {
            try
            {
                await repositorioOrganizacao
                    .Excluir(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("obter")]
        [ProducesResponseType(typeof(OrganizacaoDto), 200)]
        public async Task<IActionResult> Obter(string cnpj)
        {
            try
            {
                return Ok(await repositorioOrganizacao
                    .Obter(cnpj));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("vincular-usuario")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> VincularUsuario(string cnpjOrganizacao, string loginUsuario, bool desfazerVinculo = false)
        {
            try
            {
                return Ok(await repositorioOrganizacao
                    .VincularUsuario(loginUsuario, cnpjOrganizacao, desfazerVinculo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("listar-usuarios-atrelados")]
        public async Task<IActionResult> ListarUsuariosVinculados(string cnpjOrganizacao)
        {
            try
            {
                var resultado = await repositorioOrganizacao
                    .ListarUsuariosAtrelados(cnpjOrganizacao);

                var listaAgrupada = resultado
                    .GroupBy(x => new { x.OrganizacaoId, x.NomeOrganizacao });

                var lista = listaAgrupada.ToList()
                    .Select(l => new
                    {
                        organizacao_id = l.Key.OrganizacaoId,
                        nome_organizacao = l.Key.NomeOrganizacao,
                        usuarios = l.Select(u => new
                        {
                            usuario_id = u.UsuarioId,
                            login_usuario = u.LoginUsuario
                        })
                    });

                return Ok(lista);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
