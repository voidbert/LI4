namespace LI4.Dados;

public class OrdemProducaoRepository : IOrdemProducaoRepository
{
    private static OrdemProducaoRepository? _Instancia;

    private OrdemProducaoRepository() { }

    public static OrdemProducaoRepository Instancia
    {
        get
        {
            if (OrdemProducaoRepository._Instancia == null)
                OrdemProducaoRepository._Instancia = new OrdemProducaoRepository();
            return OrdemProducaoRepository._Instancia;
        }
    }

    public OrdemProducaoModel? Obter(int identificador)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM OrdemProducao WHERE Identificador = @identificador";
        List<OrdemProducaoModel> lista = BaseDeDados.Instancia.LerDados<OrdemProducaoModel, dynamic>(sql, new
        {
            identificador = identificador
        });

        if (lista.Count == 0)
        {
            return null;
        }

        OrdemProducaoModel model = lista[0];

        string conteudoSql = "SELECT EVA AS [Key], Quantidade AS Value FROM ConteudoOrdemProducao WHERE OrdemProducao = @encomenda";
        List<KeyValuePair<int, int>> tuplos = BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(conteudoSql, new
        {
            encomenda = identificador
        });
        model.Conteudo = new Dictionary<int, int>(tuplos);

        BaseDeDados.Instancia.CommitTransacao();
        return model;
    }

    public OrdemProducaoModel Adicionar(OrdemProducaoModel model)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "INSERT INTO OrdemProducao(Funcionario, InstanteEmissao, Visualizada) VALUES (@funcionario, @instanteEmissao, @visualizada)";
        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            funcionario = model.Funcionario,
            instanteEmissao = model.InstanteEmissao,
            visualizada = model.Visualizada
        });

        string idSql = "SELECT MAX(Identificador) FROM OrdemProducao";
        int identificador = BaseDeDados.Instancia.LerDados<int, dynamic>(idSql, new { })[0];

        string conteudosSql = "INSERT INTO ConteudoOrdemProducao (OrdemProducao, EVA, Quantidade) VALUES (@ordemProducao, @eva, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in model.Conteudo)
        {
            if (entrada.Value > 0)
            {
                BaseDeDados.Instancia.EscreverDados<dynamic>(conteudosSql, new
                {
                    ordemProducao = identificador,
                    eva = entrada.Key,
                    quantidade = entrada.Value
                });
            }
        }

        BaseDeDados.Instancia.CommitTransacao();
        return model with { Identificador = identificador };
    }

    public void Atualizar(OrdemProducaoModel model)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "UPDATE OrdemProducao SET Funcionario = @funcionario, InstanteEmissao = @instanteEmissao, Visualizada = @visualizada WHERE Identificador = @identificador";
        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            identificador = model.Identificador,
            funcionario = model.Funcionario,
            instanteEmissao = model.InstanteEmissao,
            visualizada = model.Visualizada
        });

        string apagarSql = "DELETE FROM ConteudoOrdemProducao WHERE OrdemProducao = @ordemProducao";
        BaseDeDados.Instancia.EscreverDados<dynamic>(apagarSql, new
        {
            ordemProducao = model.Identificador
        });

        string conteudosSql = "INSERT INTO ConteudoOrdemProducao (OrdemProducao, EVA, Quantidade) VALUES (@ordemProducao, @eva, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in model.Conteudo)
        {
            if (entrada.Value > 0)
            {
                BaseDeDados.Instancia.EscreverDados<dynamic>(conteudosSql, new
                {
                    ordemProducao = model.Identificador,
                    eva = entrada.Key,
                    quantidade = entrada.Value
                });
            }
        }

        BaseDeDados.Instancia.CommitTransacao();
    }
}
