using System.Text.Json.Serialization;

namespace Justific.Dominio.Dtos
{
    public class ItemListaOrganizacaoUsuarioDto
    {
        public long OrganizacaoId { get; set; }
        public string NomeOrganizacao { get; set; }
        public long UsuarioId { get; set; }
        public string LoginUsuario { get; set; }
    }
}
