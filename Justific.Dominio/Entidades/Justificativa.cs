using System;
using System.Text.Json.Serialization;

namespace Justific.Dominio.Entidades
{
    public class Justificativa : EntidadeBase
    {
        [JsonPropertyName("membro")]
        public Membro Membro { get; set; }
        [JsonPropertyName("data_ocorrencia")]
        public DateTime DataOcorrencia { get; set; }
        [JsonPropertyName("possui_comprovante")]
        public bool PossuiComprovante { get; set; }
        [JsonPropertyName("comentarios")]
        public string Comentarios { get; set; }
        [JsonPropertyName("data_criacao")]
        public DateTime DataCriacao { get; set; }
        [JsonPropertyName("alterado_em")]
        public DateTime? AlteradoEm { get; set; }
    }
}
