using Justific.Dominio.Enumeradores;
using Justific.Infra.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Justific.Data
{
    public class JustificContext : IJustificContext
    {
        private readonly DbConnection conexao;
        public JustificContext(IConfiguration configuration)
        {
            if ((TipoConexaoBD)int.Parse(configuration.GetSection("TipoConexaoBD").Value) == TipoConexaoBD.PostgreSQL)
                conexao = new NpgsqlConnection(configuration
                    .GetConnectionString("JustificConnection"));
            else
                conexao = new SqlConnection(configuration
                    .GetConnectionString("JustificConnectionSQL"));

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
