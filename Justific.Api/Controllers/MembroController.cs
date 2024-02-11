using Justific.Data.Repositorios;
using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Justific.Api.Controllers
{
    public class MembroController : BaseController<Membro>
    {
        private readonly IRepositorioMembro repositorioMembro;

        public MembroController(IRepositorioMembro repositorioMembro)
            : base (repositorioMembro)
        {
            this.repositorioMembro = repositorioMembro ?? throw new ArgumentNullException(nameof(repositorioMembro));
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                return Ok(await repositorioMembro
                    .Listar());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("obter")]
        public async Task<IActionResult> Obter(string codigoRegistro, int organizacaoId)
        {
            try
            {
                return Ok(await repositorioMembro
                    .Obter(codigoRegistro, organizacaoId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("salvar")]
        public async Task<IActionResult> Salvar(MembroInclusaoDto membroInclusaoDto)
        {
            try
            {
                return Ok(await repositorioMembro
                    .Salvar(membroInclusaoDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                await repositorioMembro
                    .Excluir(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
