namespace LI4.Dados;

public class ParteRepository : IParteRepository
{
    private static ParteRepository? _Instancia;

    private ParteRepository() { }

    public static ParteRepository Instancia
    {
        get
        {
            if (ParteRepository._Instancia == null)
                ParteRepository._Instancia = new ParteRepository();
            return ParteRepository._Instancia;
        }
    }

    public ParteModel? Obter(int identificador)
    {
        string sql = "SELECT * FROM Parte WHERE Identificador=@identificador";
        List<ParteModel> lista = BaseDeDados.Instancia.LerDados<ParteModel, dynamic>(sql, new
        {
            identificador = identificador
        });

        if (lista.Count == 0)
            return null;
        return lista[0];
    }

    public List<ParteModel> ObterTodas()
    {
        string sql = "SELECT * FROM Parte";
        List<ParteModel> lista = BaseDeDados.Instancia.LerDados<ParteModel, dynamic>(sql, new { });
        return lista;
    }

    public void Atualizar(ParteModel model)
    {
        string sql = "UPDATE Parte SET Nome=@nome, Preco=@preco, QuantidadeArmazem=@quantidadeArmazem WHERE Identificador=@identificador";

        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            identificador = model.Identificador,
            nome = model.Nome,
            preco = model.Preco,
            quantidadeArmazem = model.QuantidadeArmazem
        });
    }
}
