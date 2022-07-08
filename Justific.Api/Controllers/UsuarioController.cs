using Microsoft.AspNetCore.Mvc;

namespace Justific.Api.Controllers
{
    public class UsuarioController : BaseController
    {     
        
        [HttpPost("efetuar_login")]
        public IActionResult EfetuarLogin(string login, string senha)
        {
            return Ok();
        }
    }
}
