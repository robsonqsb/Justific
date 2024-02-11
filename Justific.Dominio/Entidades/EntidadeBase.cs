using System;
using System.Text.Json.Serialization;

namespace Justific.Dominio.Entidades
{
    public abstract class EntidadeBase
    {
        [JsonPropertyName("id")]
        public long IdRelacional { get; set; }

        [JsonPropertyName("_id")]
        public Guid IdNoSql { get; set; }
    }
}
