namespace LI4.Dados;

public class OrdemProducaoRepository
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

    public async Task<OrdemProducaoModel> Adicionar(OrdemProducaoModel model)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "INSERT INTO OrdemProducao(Funcionario, InstanteEmissao, Visualizada) VALUES (@funcionario, @instanteEmissao, @visualizada)";
        await BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            funcionario = model.Funcionario,
            instanteEmissao = model.InstanteEmissao,
            visualizada = model.Visualizada
        });

        string idSql = "SELECT MAX(Identificador) FROM OrdemProducao";
        int identificador = (await BaseDeDados.Instancia.LerDados<int, dynamic>(idSql, new { }))[0];

        string conteudosSql = "INSERT INTO ConteudoOrdemProducao (OrdemProducao, EVA, Quantidade) VALUES (@ordemProducao, @eva, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in model.Conteudo)
        {
            if (entrada.Value > 0)
            {
                await BaseDeDados.Instancia.EscreverDados<dynamic>(conteudosSql, new
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
}
