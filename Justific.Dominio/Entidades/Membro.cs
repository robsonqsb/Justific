using System;
using System.Text.Json.Serialization;

namespace Justific.Dominio.Entidades
{
    public class Membro : EntidadeBase
    {
        [JsonPropertyName("codigo_registro")]
        public string CodigoRegistro { get; set; }
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
        [JsonPropertyName("organizacao")]
        public Organizacao Organizacao { get; set; }
        [JsonPropertyName("data_criacao")]
        public DateTime DataCriacao { get; set; }
        [JsonPropertyName("alterado_em")]
        public DateTime? AlteradoEm { get; set; }
    }
}
