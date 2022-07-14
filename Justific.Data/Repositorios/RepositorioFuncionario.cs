using Dapper;
using Justific.Dominio.Entidades;
using Justific.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Justific.Data.Repositorios
{
    public class RepositorioFuncionario : RepositorioBase<Funcionario>, IRepositorioFuncionario
    {
        public RepositorioFuncionario(IJustificContext justificContext)
            : base (justificContext)
        {
        }
        public async Task Excluir(int id)
        {
            var sqlQuery = "call p_excluir_usuario(@id);";

            await justificContext
                .Conexao.ExecuteAsync(sqlQuery, new { id });
        }

        public async Task<IEnumerable<Funcionario>> Listar()
        {
            var sqlQuery = @"select f.id,
                                    f.codigo_registro CodigoRegistro,
                                    f.nome,
                                    f.organizacao_id,
                                    f.data_criacao DataCriacao,
                                    f.alterado_em AlteradoEm,
                                    o.id,
                                    o.nome,
                                    o.cnpj
                                from vw_listar_funcionarios f
                                    inner join vw_listar_organizacoes o
                                        on f.organizacao_id = o.id";

            return await justificContext
                .Conexao.QueryAsync<Funcionario, Organizacao, Funcionario>(sqlQuery, (funcionario, organizacao) =>
                {
                    return new Funcionario()
                    {
                        Id = funcionario.Id,
                        CodigoRegistro = funcionario.CodigoRegistro,
                        Nome   = funcionario.Nome,
                        DataCriacao = funcionario.DataCriacao,
                        AlteradoEm = funcionario.AlteradoEm,
                        Organizacao = new Organizacao()
                        {
                            Id = organizacao.Id,
                            Cnpj = organizacao.Cnpj,
                            Nome = organizacao.Nome
                        }
                    };
                }, splitOn: "id");
        }

        public Task<Funcionario> Obter(string codigoRegistro, int organizacaoId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> Salvar(string codigoRegistro, string nome, string cnpjOrganizacao)
        {
            throw new System.NotImplementedException();
        }
    }
}
