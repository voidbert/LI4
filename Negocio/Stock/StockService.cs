using LI4.Dados;

namespace LI4.Negocio.Stock;

public class StockService : IGestaoStock
{
    public StockService()
    {
        this.BaseDeDados = LI4.Dados.BaseDeDados.Instancia;
        this.Partes = ParteRepository.Instancia;
        this.Encomendas = EncomendaPartesRepository.Instancia;
    }

    public List<Parte> ObterTodasAsPartes()
    {
        return this.Partes.ObterTodas().Select(model => Parte.DeModel(model)).ToList();
    }

    public List<EncomendaPartes> ObterTodasAsEncomendasPartes()
    {
        return this.Encomendas.ObterTodas().Select(model => EncomendaPartes.DeModel(model)).ToList();
    }

    public void ColocarEncomendaPartes(EncomendaPartes encomenda)
    {
        encomenda.InstanteRealizacao = DateTime.Now;
        if (encomenda.Conteudo.Count == 0)
        {
            throw new EncomendaVaziaException();
        }

        this.BaseDeDados.IniciarTransacao();

        foreach (KeyValuePair<Parte, int> entrada in encomenda.Conteudo)
        {
            ParteModel parte = entrada.Key.ParaModel();
            ParteModel novaParte = parte with { QuantidadeArmazem = parte.QuantidadeArmazem + entrada.Value };
            this.Partes.Atualizar(novaParte);
        }

        EncomendaPartesModel encomendaPartesModel = encomenda.ParaModel();
        Encomendas.Adicionar(encomendaPartesModel);
        this.BaseDeDados.CommitTransacao();
    }

    private IBaseDeDados BaseDeDados;
    private IParteRepository Partes;
    private IEncomendaPartesRepository Encomendas;
}
