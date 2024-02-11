using Justific.Dominio.Dtos;
using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Justific.Api.Controllers
{
    public class JustificativaController : BaseController<Justificativa>
    {
        private readonly IRepositorioJustificativa repositorioJustificativa;

        public JustificativaController(IRepositorioJustificativa repositorioJustificativa)
            : base(repositorioJustificativa)
        {
            this.repositorioJustificativa = repositorioJustificativa ?? throw new ArgumentNullException(nameof(repositorioJustificativa));
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var itens = await repositorioJustificativa
                    .Listar();

                if (!itens.Any())
                    return NoContent();

                return Ok(itens);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("salvar")]
        public async Task<IActionResult> Salvar(JustificativaInclusaoDto justificativaInclusaoDto)
        {
            try
            {
                return Ok(await repositorioJustificativa
                    .Salvar(justificativaInclusaoDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("excluir")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                await repositorioJustificativa
                    .Excluir(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("obter")]
        public async Task<IActionResult> Obter(string codigoRegistroMembro, string cnpjOrganizacao, DateTime dataOcorrencia)
        {
            try
            {
                return Ok(await repositorioJustificativa
                    .Obter(codigoRegistroMembro, cnpjOrganizacao, dataOcorrencia));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
