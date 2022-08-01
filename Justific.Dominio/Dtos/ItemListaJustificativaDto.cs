using System;

namespace Justific.Dominio.Dtos
{
    public class ItemListaJustificativaDto
    {
        public long JustificativaId { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public bool PossuiComprovante { get; set; }
        public string Comentarios { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public long MembroId { get; set; }
        public string CodigoRegistro { get; set; }
        public string NomeMembro { get; set; }
        public long OrganizacaoId { get; set; }
        public string NomeOrganizacao { get; set; }
        public string CNPJ { get; set; }
    }
}
