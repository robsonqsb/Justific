using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Justific.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : Controller where T: EntidadeBase
    {
        protected readonly IRepositorioBase<T> repositorio;

        public BaseController(IRepositorioBase<T> repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
    }
}
