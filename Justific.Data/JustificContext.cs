using Justific.Infra.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace Justific.Data
{
    public class JustificContext : IJustificContext
    {
        private readonly NpgsqlConnection conexao;

        public JustificContext(IConfiguration configuration)
        {
            conexao = new NpgsqlConnection(configuration
                .GetConnectionString("JustificConnection"));

            if (conexao.State == ConnectionState.Closed)
                conexao.Open();
        }

        public IDbConnection Conexao => conexao;

        public async Task Dispose()
        {
            if (conexao.State != ConnectionState.Closed)
                await conexao.CloseAsync();

            await conexao.DisposeAsync();
        }
    }
}
