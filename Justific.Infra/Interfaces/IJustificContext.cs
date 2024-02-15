using System.Data;
using System.Threading.Tasks;

namespace Justific.Infra.Interfaces
{
    public interface IJustificContext
    {
        Task Dispose();
        IDbConnection Conexao { get; }
    }
}
