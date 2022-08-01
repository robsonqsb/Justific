using System;

namespace Justific.Dominio.Dtos
{
    public class ItemListaMembroOrganizacaoDto
    {
        public long MembroId { get; set; }
        public string CodigoRegistro { get; set; }
        public string NomeMembro { get; set; }
        public DateTime DataCriacaoMembro { get; set; }
        public DateTime? MembroAlteradoEm { get; set; }
        public long OrganizacaoId { get; set; }
        public string NomeOrganizacao { get; set; }
        public string Cnpj { get; set; }
    }
}
