using System;
using System.Text.Json.Serialization;

namespace Justific.Dominio.Entidades
{    
    public class Usuario : EntidadeBase
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("data_criacao")]
        public DateTime DataCriacao { get; set; }
        [JsonPropertyName("alterado_em")]
        public DateTime? AlteradoEm { get; set; }
        [JsonPropertyName("senha")]
        public string Senha { get; set; }

    }
}
