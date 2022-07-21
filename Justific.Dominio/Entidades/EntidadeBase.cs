using System.Text.Json.Serialization;

namespace Justific.Dominio.Entidades
{
    public class EntidadeBase
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
