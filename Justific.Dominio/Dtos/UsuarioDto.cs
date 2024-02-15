using System;
using System.Text.Json.Serialization;

namespace Justific.Dominio.Dtos
{
    public class UsuarioDto : BaseDto
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }
        [JsonPropertyName("data_criacao")]
        public DateTime DataCriacao { get; set; }
        [JsonPropertyName("alterado_em")]
        public DateTime? AlteradoEm { get; set; }
    }
}
