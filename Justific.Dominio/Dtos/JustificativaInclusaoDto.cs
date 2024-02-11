using System;

namespace Justific.Dominio.Dtos
{
    public class JustificativaInclusaoDto : BaseDto
    {
        public JustificativaInclusaoDto(string codigoRegistroMembro, string cnpjOrganizacao, string comentarios, DateTime? dataOcorrencia, bool? possuiComprovante)
        {
            CodigoRegistroMembro = codigoRegistroMembro;
            CnpjOrganizacao = cnpjOrganizacao;
            Comentarios = comentarios;
            DataOcorrencia = dataOcorrencia ?? DateTime.UtcNow;
            PossuiComprovante = possuiComprovante;
        }

        public string CodigoRegistroMembro { get; private set; }
        public string CnpjOrganizacao { get; private set; }
        public string Comentarios { get; private set; }
        public DateTime? DataOcorrencia { get; private set; }
        public bool? PossuiComprovante { get; private set; }
    }
}
