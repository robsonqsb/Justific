using System;

namespace Justific.Dominio.Entidades
{
    public class Usuario : EntidadeBase
    {
        public string Login { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}
