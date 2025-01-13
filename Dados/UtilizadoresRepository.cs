namespace LI4.Dados;

public class UtilizadoresRepository
{
    private static UtilizadoresRepository? _Instancia;

    private UtilizadoresRepository() { }

    public static UtilizadoresRepository Instancia
    {
        get
        {
            if (UtilizadoresRepository._Instancia == null)
                UtilizadoresRepository._Instancia = new UtilizadoresRepository();
            return UtilizadoresRepository._Instancia;
        }
    }

    public async Task<UtilizadorModel?> Obter(string enderecoEletronico)
    {
        string sql = "SELECT * FROM Utilizador WHERE EnderecoEletronico=@enderecoEletronico";
        List<UtilizadorModel> lista = await BaseDeDados.Instancia.LerDados<UtilizadorModel, dynamic>(sql, new
        {
            enderecoEletronico = enderecoEletronico
        });

        if (lista.Count == 0)
            return null;
        return lista[0];
    }

    public async Task<List<UtilizadorModel>> ObterTodos()
    {
        string sql = "SELECT * FROM Utilizador";
        List<UtilizadorModel> lista = await BaseDeDados.Instancia.LerDados<UtilizadorModel, dynamic>(sql, new { });
        return lista;
    }

    public async Task Adicionar(UtilizadorModel model)
    {
        string sql = "INSERT INTO Utilizador (EnderecoEletronico, NomeCivil, PalavraPasse, TipoDeConta, PossivelIniciarSessao) VALUES (@enderecoEletronico, @nomeCivil, @palavraPasse, @tipoDeConta, @possivelIniciarSessao)";

        await BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            enderecoEletronico = model.EnderecoEletronico,
            nomeCivil = model.NomeCivil,
            palavraPasse = model.PalavraPasse,
            tipoDeConta = model.TipoDeConta,
            possivelIniciarSessao = model.PossivelIniciarSessao
        });
    }

    public async Task Atualizar(UtilizadorModel model)
    {
        string sql = "UPDATE Utilizador SET NomeCivil=@nomeCivil, PalavraPasse=@palavraPasse, TipoDeConta=@tipoDeConta, PossivelIniciarSessao=@PossivelIniciarSessao WHERE EnderecoEletronico=@enderecoEletronico";

        await BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            enderecoEletronico = model.EnderecoEletronico,
            nomeCivil = model.NomeCivil,
            palavraPasse = model.PalavraPasse,
            tipoDeConta = model.TipoDeConta,
            possivelIniciarSessao = model.PossivelIniciarSessao
        });
    }
}
