using System;
using System.Text.Json.Serialization;

namespace Justific.Dominio.Entidades
{
    public class Organizacao : EntidadeBase
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }
        [JsonPropertyName("data_criacao")]
        public DateTime DataCriacao { get; set; }
        [JsonPropertyName("alterado_em")]
        public DateTime? AlteradoEm { get; set; }
    }
}
