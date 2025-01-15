namespace LI4.Dados;

public class PartesRepository
{
    private static PartesRepository? _Instancia;

    private PartesRepository() { }

    public static PartesRepository Instancia
    {
        get
        {
            if (PartesRepository._Instancia == null)
                PartesRepository._Instancia = new PartesRepository();
            return PartesRepository._Instancia;
        }
    }

    public async Task<ParteModel?> Obter(int identificador)
    {
        string sql = "SELECT * FROM Parte WHERE Identificador=@identificador";
        List<ParteModel> lista = await BaseDeDados.Instancia.LerDados<ParteModel, dynamic>(sql, new
        {
            identificador = identificador
        });

        if (lista.Count == 0)
            return null;
        return lista[0];
    }

    public async Task<List<ParteModel>> ObterTodas()
    {
        string sql = "SELECT * FROM Parte";
        List<ParteModel> lista = await BaseDeDados.Instancia.LerDados<ParteModel, dynamic>(sql, new { });
        return lista;
    }

    public async Task Atualizar(ParteModel model)
    {
        string sql = "UPDATE Parte SET Nome=@nome, Preco=@preco, QuantidadeArmazem=@quantidadeArmazem WHERE Identificador=@identificador";

        await BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            identificador = model.Identificador,
            nome = model.Nome,
            preco = model.Preco,
            quantidadeArmazem = model.QuantidadeArmazem
        });
    }
}
