using Justific.Dominio.Entidades;
using Justific.Dominio.Interfaces.Repositorios;
using Justific.Infra.Interfaces;
using System;

namespace Justific.Data.Repositorios
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T: EntidadeBase
    {
        protected readonly IJustificContext justificContext;

        public RepositorioBase(IJustificContext justificContext)
        {
            this.justificContext = justificContext ?? throw new ArgumentNullException(nameof(justificContext));
        }
    }
}
