using Justific.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace Justific.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : Controller where T : EntidadeBase
    {
    }
}
