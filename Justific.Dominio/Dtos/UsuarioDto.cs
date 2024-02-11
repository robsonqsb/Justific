using System;

namespace Justific.Dominio.Dtos
{
    public class UsuarioDto : BaseDto
    {
        public string Login { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}
