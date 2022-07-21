using System.Text.Json.Serialization;

namespace Justific.Dominio.Dtos
{
    public class ItemListaOrganizacaoUsuarioDto
    {
        public int OrganizacaoId { get; set; }
        public string NomeOrganizacao { get; set; }
        public int UsuarioId { get; set; }
        public string LoginUsuario { get; set; }
    }
}
