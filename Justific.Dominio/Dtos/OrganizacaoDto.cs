using System;

namespace Justific.Dominio.Dtos
{
    public class OrganizacaoDto : BaseDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? AlteradoEm { get; set; }
    }
}
