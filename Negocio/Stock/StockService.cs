using LI4.Dados;

namespace LI4.Negocio.Stock;

public class StockService
{
    public Parte? ObterParte(int identificador)
    {
        ParteModel? model = ParteRepository.Instancia.Obter(identificador);
        if (model == null)
        {
            return null;
        }

        return Parte.DeModel(model);
    }

    public List<Parte> ObterTodasAsPartes()
    {
        return ParteRepository.Instancia.ObterTodas().Select(model => Parte.DeModel(model)).ToList();
    }

    public List<EncomendaPartes> ObterTodasAsEncomendasPartes()
    {
        return EncomendaPartesRepository.Instancia.ObterTodas().Select(model => EncomendaPartes.DeModel(model)).ToList();
    }

    public void ColocarEncomenda(EncomendaPartes encomenda)
    {
        if (encomenda.Conteudo.Count == 0)
        {
            throw new EncomendaVaziaException();
        }

        BaseDeDados.Instancia.IniciarTransacao();

        foreach (KeyValuePair<Parte, int> entrada in encomenda.Conteudo)
        {
            ParteModel parte = entrada.Key.ParaModel();
            ParteModel novaParte = parte with { QuantidadeArmazem = parte.QuantidadeArmazem + entrada.Value };
            ParteRepository.Instancia.Atualizar(novaParte);
        }

        EncomendaPartesModel encomendaPartesModel = encomenda.ParaModel();
        EncomendaPartesRepository.Instancia.Adicionar(encomendaPartesModel);
        BaseDeDados.Instancia.CommitTransacao();
    }
}
