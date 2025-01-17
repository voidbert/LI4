using LI4.Dados;

namespace LI4.Negocio.Encomendas;

public class EncomendasService
{
    public CarrinhoCompras ObterCarrinho(string cliente)
    {
        return CarrinhoCompras.DeModel(CarrinhoComprasRepository.Instancia.Obter(cliente));
    }

    public List<EncomendaEVAs> ObterTodasAsEncomendasEVAs()
    {
        return EncomendaEVAsRepository.Instancia.ObterTodas().Select(model => EncomendaEVAs.DeModel(model)).ToList();
    }

    public void AtualizarCarrinho(CarrinhoCompras carrinho)
    {
        CarrinhoComprasRepository.Instancia.Atualizar(carrinho.ParaModel());
    }

    public void ColocarEncomenda(EncomendaEVAs encomenda)
    {
        if (encomenda.Conteudo.Count == 0)
        {
            throw new CarrinhoVazioException();
        }

        EncomendaEVAsRepository.Instancia.Adicionar(encomenda.ParaModel());
    }

    public void RejeitarEncomenda(int identificador)
    {
        EncomendaEVAsModel? model = EncomendaEVAsRepository.Instancia.Obter(identificador);
        if (model == null)
        {
            throw new EncomendaNaoEncontradaException();
        }

        EncomendaEVAs encomenda = EncomendaEVAs.DeModel(model);

        if (encomenda.Estado != EncomendaEVAs.EstadoEncomenda.Colocada)
        {
            throw new EstadoInvalidoException();
        }

        encomenda.Aprovada = false;
        encomenda.InstanteConfirmacao = DateTime.Now;
        EncomendaEVAsRepository.Instancia.Atualizar(encomenda.ParaModel());
    }

    public void AprovarEncomenda(int identificador)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        EncomendaEVAsModel? model = EncomendaEVAsRepository.Instancia.Obter(identificador);
        if (model == null)
        {
            BaseDeDados.Instancia.AbortarTransacao();
            throw new EncomendaNaoEncontradaException();
        }

        EncomendaEVAs encomenda = EncomendaEVAs.DeModel(model);

        if (encomenda.Estado != EncomendaEVAs.EstadoEncomenda.Colocada)
        {
            BaseDeDados.Instancia.AbortarTransacao();
            throw new EstadoInvalidoException();
        }

        encomenda.Aprovada = true;
        encomenda.InstanteConfirmacao = DateTime.Now;
        this.TentarSatisfazerEncomenda(encomenda);
        EncomendaEVAsRepository.Instancia.Atualizar(encomenda.ParaModel());

        BaseDeDados.Instancia.CommitTransacao();
    }

    private void TentarSatisfazerEncomenda(EncomendaEVAs encomenda)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        Dictionary<int, EVAModel> evas = EVARepository.Instancia.ObterTodas().ToDictionary(eva => eva.Identificador, eva => eva);
        bool podeSatisfazer = encomenda.ConteudoRaw.All(entrada => evas[entrada.Key].QuantidadeArmazem >= entrada.Value);

        if (podeSatisfazer)
        {
            foreach (EVA eva in encomenda.Conteudo.Keys)
            {
                eva.QuantidadeArmazem -= encomenda.ConteudoRaw[eva.Identificador];
                EVARepository.Instancia.Atualizar(eva.ParaModel());
            }

            encomenda.InstanteEntrega = DateTime.Now;
        }

        BaseDeDados.Instancia.CommitTransacao();
    }
}
