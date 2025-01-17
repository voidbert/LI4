namespace LI4.Dados;

public class EncomendaEVAsRepository
{
    private static EncomendaEVAsRepository? _Instancia;

    private EncomendaEVAsRepository() { }

    public static EncomendaEVAsRepository Instancia
    {
        get
        {
            if (EncomendaEVAsRepository._Instancia == null)
                EncomendaEVAsRepository._Instancia = new EncomendaEVAsRepository();
            return EncomendaEVAsRepository._Instancia;
        }
    }

    public EncomendaEVAsModel? Obter(int identificador)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM EncomendaEVAs WHERE Identificador = @identificador";
        List<EncomendaEVAsModel> lista = BaseDeDados.Instancia.LerDados<EncomendaEVAsModel, dynamic>(sql, new
        {
            identificador = identificador
        });

        if (lista.Count == 0)
        {
            return null;
        }

        EncomendaEVAsModel model = lista[0];

        string conteudoSql = "SELECT EVA AS [Key], Quantidade AS Value FROM ConteudoEncomendaEVAs WHERE Encomenda = @encomenda";
        List<KeyValuePair<int, int>> tuplos = BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(conteudoSql, new
        {
            encomenda = identificador
        });
        model.Conteudo = new Dictionary<int, int>(tuplos);

        BaseDeDados.Instancia.CommitTransacao();
        return model;
    }

    public List<EncomendaEVAsModel> ObterTodas()
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM EncomendaEVAs";
        List<EncomendaEVAsModel> lista = BaseDeDados.Instancia.LerDados<EncomendaEVAsModel, dynamic>(sql, new { });

        string conteudoSql = "SELECT EVA AS [Key], Quantidade AS Value FROM ConteudoEncomendaEVAs WHERE Encomenda = @encomenda";
        foreach (EncomendaEVAsModel model in lista)
        {
            List<KeyValuePair<int, int>> tuplos = BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(conteudoSql, new
            {
                encomenda = model.Identificador
            });

            model.Conteudo = new Dictionary<int, int>(tuplos);
        }

        BaseDeDados.Instancia.CommitTransacao();
        return lista;
    }

    public EncomendaEVAsModel Adicionar(EncomendaEVAsModel model)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "INSERT INTO EncomendaEVAs(Cliente, Morada, Preco, InstanteColocacao, InstanteConfirmacao, InstanteEntrega, InstanteCancelamento, InstanteDevolucao, Aprovada) VALUES (@cliente, @morada, @preco, @instanteColocacao, @instanteConfirmacao, @instanteEntrega, @instanteCancelamento, @instanteDevolucao, @aprovada)";
        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            cliente = model.Cliente,
            morada = model.Morada,
            preco = model.Preco,
            instanteColocacao = model.InstanteColocacao,
            instanteConfirmacao = model.InstanteConfirmacao,
            instanteEntrega = model.InstanteEntrega,
            instanteCancelamento = model.InstanteCancelamento,
            instanteDevolucao = model.InstanteDevolucao,
            aprovada = model.Aprovada
        });

        string idSql = "SELECT MAX(Identificador) FROM EncomendaEVAs";
        int identificador = BaseDeDados.Instancia.LerDados<int, dynamic>(idSql, new { })[0];

        string conteudosSql = "INSERT INTO ConteudoEncomendaEVAs (Encomenda, EVA, Quantidade) VALUES (@encomenda, @eva, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in model.Conteudo)
        {
            if (entrada.Value > 0)
            {
                BaseDeDados.Instancia.EscreverDados<dynamic>(conteudosSql, new
                {
                    encomenda = identificador,
                    eva = entrada.Key,
                    quantidade = entrada.Value
                });
            }
        }

        BaseDeDados.Instancia.CommitTransacao();
        return model with { Identificador = identificador };
    }

    public void Atualizar(EncomendaEVAsModel model)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "UPDATE EncomendaEVAs SET Cliente = @cliente, Morada = @morada, Preco = @preco, InstanteColocacao = @instanteColocacao, InstanteConfirmacao = @instanteConfirmacao, InstanteEntrega = @instanteEntrega, InstanteCancelamento = @instanteCancelamento, InstanteDevolucao = @instanteDevolucao, Aprovada = @aprovada WHERE Identificador = @identificador";
        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            identificador = model.Identificador,
            cliente = model.Cliente,
            morada = model.Morada,
            preco = model.Preco,
            instanteColocacao = model.InstanteColocacao,
            instanteConfirmacao = model.InstanteConfirmacao,
            instanteEntrega = model.InstanteEntrega,
            instanteCancelamento = model.InstanteCancelamento,
            instanteDevolucao = model.InstanteDevolucao,
            aprovada = model.Aprovada
        });

        string apagarSql = "DELETE FROM ConteudoEncomendaEVAs WHERE Encomenda = @encomenda";
        BaseDeDados.Instancia.EscreverDados<dynamic>(apagarSql, new
        {
            encomenda = model.Identificador
        });

        string conteudosSql = "INSERT INTO ConteudoEncomendaEVAs (Encomenda, EVA, Quantidade) VALUES (@encomenda, @eva, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in model.Conteudo)
        {
            if (entrada.Value > 0)
            {
                BaseDeDados.Instancia.EscreverDados<dynamic>(conteudosSql, new
                {
                    encomenda = model.Identificador,
                    eva = entrada.Key,
                    quantidade = entrada.Value
                });
            }
        }

        BaseDeDados.Instancia.CommitTransacao();
    }

    public void Eliminar(int identificador)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string conteudosSql = "DELETE FROM ConteudoEncomendaEVAs WHERE Encomenda = @encomenda";
        BaseDeDados.Instancia.EscreverDados<dynamic>(conteudosSql, new
        {
            encomenda = identificador
        });

        string sql = "DELETE FROM EncomendaEVAs WHERE Identificador = @identificador";
        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            identificador = identificador
        });

        BaseDeDados.Instancia.CommitTransacao();
    }
}
