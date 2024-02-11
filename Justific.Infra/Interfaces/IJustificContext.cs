using System.Data;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Justific.Infra.Interfaces
{
    public interface IJustificContext
    {
        Task Dispose();
        IDbConnection Conexao { get; }
        IMongoClient MongoClient { get; }
    }
}
