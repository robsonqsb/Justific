using Justific.Dominio.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Justific.Api.Controllers
{
    public class UsuarioController : BaseController
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public UsuarioController(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
        }

        [HttpPost("efetuar_login")]
        public async Task<IActionResult> EfetuarLogin(string login, string senha)
        {
            try
            {
                var confirmado = await repositorioUsuario
                    .ConfirmarLogin(login, senha);

                if (confirmado)
                    return Ok();

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("excluir")]
        public async Task<IActionResult> Excluir(long usuarioId)
        {
            try
            {
                await repositorioUsuario
                    .Excluir(usuarioId);

                return Ok();
            }
            catch (PostgresException pex)
            {
                return BadRequest(pex.MessageText);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("salvar")]
        public async Task<IActionResult> Salvar(string login, string senha)
        {
            try
            {
                var id = await repositorioUsuario
                    .IncluirAlterar(login, senha);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                return Ok(await repositorioUsuario
                    .Listar());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("obter")]
        public async Task<IActionResult> Obter(string login)
        {
            try
            {
                var resultado = await repositorioUsuario
                    .Obter(login);

                if (resultado != null)
                    return Ok(resultado);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
