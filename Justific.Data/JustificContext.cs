using Justific.Dominio.Enumeradores;
using Justific.Infra.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Justific.Data
{
    public class JustificContext : IJustificContext
    {
        private readonly DbConnection conexao;
        private readonly IMongoClient mongoClient;

        public JustificContext(IConfiguration configuration)
        {
            var tipoConexao = (TipoConexaoBD)int.Parse(configuration.GetSection("TipoConexaoBD").Value);

            switch (tipoConexao)
            {
                case TipoConexaoBD.SQL_Server:
                    conexao = new SqlConnection(configuration
                        .GetConnectionString("JustificConnectionSQL"));
                    break;
                case TipoConexaoBD.MongoDB:
                    mongoClient = new MongoClient(configuration
                        .GetConnectionString("JustificConnectionMongoDB"));
                    return;
                case TipoConexaoBD.PostgreSQL:
                default:
                    conexao = new NpgsqlConnection(configuration
                        .GetConnectionString("JustificConnection"));
                    break;
            }

            if (conexao.State == ConnectionState.Closed)
                conexao.Open();
        }

        public IDbConnection Conexao => conexao ?? throw new InvalidOperationException("A conexão do banco relacional não foi iniciada.");
        public IMongoClient MongoClient => mongoClient ?? throw new InvalidOperationException("A conexão do banco noSQL não foi iniciada.");

        public async Task Dispose()
        {
            if (conexao != null)
            {
                if (conexao.State != ConnectionState.Closed)
                    await conexao.CloseAsync();

                await conexao.DisposeAsync();
            }
        }
    }
}
