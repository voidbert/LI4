namespace LI4.Dados;

public class EVARepository
{
    private static EVARepository? _Instancia;

    private EVARepository() { }

    public static EVARepository Instancia
    {
        get
        {
            if (EVARepository._Instancia == null)
                EVARepository._Instancia = new EVARepository();
            return EVARepository._Instancia;
        }
    }

    public async Task<List<EVAModel>> ObterTodas()
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM EVA";
        List<EVAModel> lista = await BaseDeDados.Instancia.LerDados<EVAModel, dynamic>(sql, new { });

        string conteudoSql = "SELECT Parte AS [Key], Quantidade AS Value FROM EVAPartes WHERE EVA = @eva";
        foreach (EVAModel model in lista)
        {
            List<KeyValuePair<int, int>> tuplos = await BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(conteudoSql, new
            {
                eva = model.Identificador
            });

            model.Partes = new Dictionary<int, int>(tuplos);
        }

        BaseDeDados.Instancia.CommitTransacao();
        return lista;
    }
}
