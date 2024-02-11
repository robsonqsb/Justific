namespace Justific.Dominio.Dtos
{
    public class MembroInclusaoDto : BaseDto
    {
        public MembroInclusaoDto(string codigoRegistro, string nome, string cnpjOrganizacao)
        {
            CodigoRegistro = codigoRegistro;
            Nome = nome;
            CnpjOrganizacao = cnpjOrganizacao;
        }

        public string CodigoRegistro { get; private set; }
        public string Nome { get; private set; }
        public string CnpjOrganizacao { get; private set; }
    }
}
