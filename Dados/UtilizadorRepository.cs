namespace LI4.Dados;

public class UtilizadorRepository
{
    private static UtilizadorRepository? _Instancia;

    private UtilizadorRepository() { }

    public static UtilizadorRepository Instancia
    {
        get
        {
            if (UtilizadorRepository._Instancia == null)
                UtilizadorRepository._Instancia = new UtilizadorRepository();
            return UtilizadorRepository._Instancia;
        }
    }

    public UtilizadorModel? Obter(string enderecoEletronico)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM Utilizador WHERE EnderecoEletronico=@enderecoEletronico";
        List<UtilizadorModel> lista = BaseDeDados.Instancia.LerDados<UtilizadorModel, dynamic>(sql, new
        {
            enderecoEletronico = enderecoEletronico
        });

        if (lista.Count == 0)
        {
            return null;
        }

        UtilizadorModel model = lista[0];

        if (model.TipoDeConta == "C")
        {
            string conteudoSql = "SELECT Identificador FROM EncomendaEVAs WHERE Cliente = @cliente";
            List<int> encomendas = BaseDeDados.Instancia.LerDados<int, dynamic>(conteudoSql, new
            {
                cliente = enderecoEletronico
            });

            model = model with { Encomendas = encomendas };
        }
        else if (model.TipoDeConta == "GP")
        {
            string conteudoSql = "SELECT Identificador FROM OrdemProducao WHERE Funcionario = @funcionario";
            List<int> ordensProducao = BaseDeDados.Instancia.LerDados<int, dynamic>(conteudoSql, new
            {
                funcionario = enderecoEletronico
            });

            model = model with { OrdensProducao = ordensProducao };
        }

        BaseDeDados.Instancia.CommitTransacao();
        return model;
    }

    public List<UtilizadorModel> ObterTodos()
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM Utilizador";
        List<UtilizadorModel> lista = BaseDeDados.Instancia.LerDados<UtilizadorModel, dynamic>(sql, new { });

        for (int i = 0; i < lista.Count; ++i)
        {
            if (lista[i].TipoDeConta == "C")
            {
                string conteudoSql = "SELECT Identificador FROM EncomendaEVAs WHERE Cliente = @cliente";
                List<int> encomendas = BaseDeDados.Instancia.LerDados<int, dynamic>(conteudoSql, new
                {
                    cliente = lista[i].EnderecoEletronico
                });

                lista[i] = lista[i] with { Encomendas = encomendas };
            }
            else if (lista[i].TipoDeConta == "GP")
            {
                string conteudoSql = "SELECT Identificador FROM OrdemProducao WHERE Funcionario = @funcionario";
                List<int> ordensProducao = BaseDeDados.Instancia.LerDados<int, dynamic>(conteudoSql, new
                {
                    funcionario = lista[i].EnderecoEletronico
                });

                lista[i] = lista[i] with { OrdensProducao = ordensProducao };
            }
        }

        BaseDeDados.Instancia.CommitTransacao();
        return lista;
    }

    public void Adicionar(UtilizadorModel model)
    {
        string sql = "INSERT INTO Utilizador (EnderecoEletronico, NomeCivil, PalavraPasse, TipoDeConta, PossivelIniciarSessao) VALUES (@enderecoEletronico, @nomeCivil, @palavraPasse, @tipoDeConta, @possivelIniciarSessao)";

        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            enderecoEletronico = model.EnderecoEletronico,
            nomeCivil = model.NomeCivil,
            palavraPasse = model.PalavraPasse,
            tipoDeConta = model.TipoDeConta,
            possivelIniciarSessao = model.PossivelIniciarSessao
        });
    }

    public void Atualizar(UtilizadorModel model)
    {
        string sql = "UPDATE Utilizador SET NomeCivil=@nomeCivil, PalavraPasse=@palavraPasse, TipoDeConta=@tipoDeConta, PossivelIniciarSessao=@PossivelIniciarSessao WHERE EnderecoEletronico=@enderecoEletronico";

        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            enderecoEletronico = model.EnderecoEletronico,
            nomeCivil = model.NomeCivil,
            palavraPasse = model.PalavraPasse,
            tipoDeConta = model.TipoDeConta,
            possivelIniciarSessao = model.PossivelIniciarSessao
        });
    }
}
