namespace LI4.Dados;

public class UtilizadoresRepository
{
    private static UtilizadoresRepository? _Instance;

    private UtilizadoresRepository() { }

    public static UtilizadoresRepository Instance
    {
        get
        {
            if (UtilizadoresRepository._Instance == null)
                UtilizadoresRepository._Instance = new UtilizadoresRepository();
            return UtilizadoresRepository._Instance;
        }
    }

    public async Task<UtilizadorModel?> Get(string enderecoEletronico)
    {
        string sql = "SELECT * FROM Utilizador WHERE EnderecoEletronico=@enderecoEletronico";
        List<UtilizadorModel> lista = await Database.Instance.LoadData<UtilizadorModel, dynamic>(sql, new
        {
            enderecoEletronico = enderecoEletronico
        });

        if (lista.Count == 0)
            return null;
        return lista[0];
    }

    public async Task<List<UtilizadorModel>> GetAll()
    {
        string sql = "SELECT * FROM Utilizador";
        List<UtilizadorModel> lista = await Database.Instance.LoadData<UtilizadorModel, dynamic>(sql, new { });
        return lista;
    }

    public async Task Add(UtilizadorModel model)
    {
        string sql = "INSERT INTO Utilizador (EnderecoEletronico, NomeCivil, PalavraPasse, TipoDeConta, PossivelIniciarSessao) VALUES (@enderecoEletronico, @nomeCivil, @palavraPasse, @tipoDeConta, @possivelIniciarSessao)";

        await Database.Instance.SaveData<dynamic>(sql, new
        {
            enderecoEletronico = model.EnderecoEletronico,
            nomeCivil = model.NomeCivil,
            palavraPasse = model.PalavraPasse,
            tipoDeConta = model.TipoDeConta,
            possivelIniciarSessao = model.PossivelIniciarSessao
        });
    }

    public async Task Update(UtilizadorModel model)
    {

        string sql = "UPDATE Utilizador SET NomeCivil=@nomeCivil, PalavraPasse=@palavraPasse, TipoDeConta=@tipoDeConta, PossivelIniciarSessao=@PossivelIniciarSessao WHERE EnderecoEletronico=@enderecoEletronico";

        await Database.Instance.SaveData<dynamic>(sql, new
        {
            enderecoEletronico = model.EnderecoEletronico,
            nomeCivil = model.NomeCivil,
            palavraPasse = model.PalavraPasse,
            tipoDeConta = model.TipoDeConta,
            possivelIniciarSessao = model.PossivelIniciarSessao
        });
    }
}
