using LI4.Dados;
using LI4.Negocio.Stock;

namespace LI4.Negocio.Producao;

public class ProducaoService : IGestaoProducao
{
    public ProducaoService()
    {
        this.BaseDeDados = LI4.Dados.BaseDeDados.Instancia;
        this.CarrinhosCompras = CarrinhoComprasRepository.Instancia;
        this.Encomendas = EncomendaEVAsRepository.Instancia;
        this.EVAs = EVARepository.Instancia;
        this.Partes = ParteRepository.Instancia;
        this.OrdensProducao = OrdemProducaoRepository.Instancia;
    }

    public CarrinhoCompras ObterCarrinho(string cliente)
    {
        return CarrinhoCompras.DeModel(this.CarrinhosCompras.Obter(cliente));
    }

    public List<EncomendaEVAs> ObterTodasAsEncomendasEVAs()
    {
        return this.Encomendas.ObterTodas().Select(model => EncomendaEVAs.DeModel(model)).ToList();
    }

    public void AtualizarCarrinho(CarrinhoCompras carrinho)
    {
        this.CarrinhosCompras.Atualizar(carrinho.ParaModel());
    }

    public void ColocarEncomendaEVAs(EncomendaEVAs encomenda)
    {
        if (encomenda.Conteudo.Count == 0)
        {
            throw new CarrinhoVazioException();
        }

        encomenda.InstanteColocacao = DateTime.Now;
        this.Encomendas.Adicionar(encomenda.ParaModel());
    }

    public void RejeitarEncomendaEVAs(int identificador)
    {
        this.BaseDeDados.IniciarTransacao();

        EncomendaEVAsModel? model = this.Encomendas.Obter(identificador);
        if (model == null)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new EncomendaNaoEncontradaException();
        }

        EncomendaEVAs encomenda = EncomendaEVAs.DeModel(model);

        if (encomenda.Estado != EncomendaEVAs.EstadoEncomenda.Colocada)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new EstadoInvalidoException();
        }

        encomenda.Aprovada = false;
        encomenda.InstanteConfirmacao = DateTime.Now;
        this.Encomendas.Atualizar(encomenda.ParaModel());

        this.BaseDeDados.CommitTransacao();
    }

    public void AprovarEncomendaEVAs(int identificador)
    {
        this.BaseDeDados.IniciarTransacao();

        EncomendaEVAsModel? model = this.Encomendas.Obter(identificador);
        if (model == null)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new EncomendaNaoEncontradaException();
        }

        EncomendaEVAs encomenda = EncomendaEVAs.DeModel(model);

        if (encomenda.Estado != EncomendaEVAs.EstadoEncomenda.Colocada)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new EstadoInvalidoException();
        }

        encomenda.Aprovada = true;
        encomenda.InstanteConfirmacao = DateTime.Now;
        this.TentarSatisfazerEncomenda(encomenda);
        this.Encomendas.Atualizar(encomenda.ParaModel());

        this.BaseDeDados.CommitTransacao();
    }

    public void CancelarEncomendaEVAs(int identificador)
    {
        this.BaseDeDados.IniciarTransacao();

        EncomendaEVAsModel? model = this.Encomendas.Obter(identificador);
        if (model == null)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new EncomendaNaoEncontradaException();
        }

        EncomendaEVAs encomenda = EncomendaEVAs.DeModel(model);

        if (encomenda.Estado != EncomendaEVAs.EstadoEncomenda.Colocada && encomenda.Estado != EncomendaEVAs.EstadoEncomenda.Aprovada)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new EstadoInvalidoException();
        }

        encomenda.InstanteCancelamento = DateTime.Now;
        this.Encomendas.Atualizar(encomenda.ParaModel());
        this.BaseDeDados.CommitTransacao();
    }

    public void DevolverEncomendaEVAs(int identificador)
    {
        this.BaseDeDados.IniciarTransacao();

        EncomendaEVAsModel? model = this.Encomendas.Obter(identificador);
        if (model == null)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new EncomendaNaoEncontradaException();
        }

        EncomendaEVAs encomenda = EncomendaEVAs.DeModel(model);

        if (encomenda.Estado != EncomendaEVAs.EstadoEncomenda.Entregue || (DateTime.Now - encomenda.InstanteEntrega)!.Value.TotalDays > (365 * 3))
        {
            this.BaseDeDados.AbortarTransacao();
            throw new EstadoInvalidoException();
        }

        encomenda.InstanteDevolucao = DateTime.Now;
        this.Encomendas.Atualizar(encomenda.ParaModel());

        foreach (KeyValuePair<EVA, int> entrada in encomenda.Conteudo)
        {
            entrada.Key.QuantidadeArmazem += entrada.Value;
            this.EVAs.Atualizar(entrada.Key.ParaModel());
        }

        this.TentarSatisfazerTodasAsEncomendas();
        this.BaseDeDados.CommitTransacao();
    }

    public void ColocarOrdemProducao(OrdemProducao ordemProducao)
    {
        if (ordemProducao.Conteudo.Count == 0)
            throw new OrdemProducaoVaziaException();

        this.BaseDeDados.IniciarTransacao();

        // Remover partes necessárias e colocar novas EVAs no armazém
        foreach (KeyValuePair<EVA, int> evaEntrada in ordemProducao.Conteudo)
        {
            EVAModel eva = evaEntrada.Key.ParaModel();

            foreach (KeyValuePair<Parte, int> parteEntrada in evaEntrada.Key.Partes)
            {
                ParteModel parte = parteEntrada.Key.ParaModel();

                if (parteEntrada.Key.QuantidadeArmazem < parteEntrada.Value * evaEntrada.Value)
                {
                    this.BaseDeDados.AbortarTransacao();
                    throw new SemPartesException();
                }

                ParteModel novaParte = parte with { QuantidadeArmazem = parte.QuantidadeArmazem - parteEntrada.Value * evaEntrada.Value };
                this.Partes.Atualizar(novaParte);
            }

            EVAModel novaEVA = eva with { QuantidadeArmazem = eva.QuantidadeArmazem + evaEntrada.Value };
            this.EVAs.Atualizar(novaEVA);
        }

        // Registar ordem de producao
        ordemProducao.InstanteEmissao = DateTime.Now;
        this.OrdensProducao.Adicionar(ordemProducao.ParaModel());

        this.TentarSatisfazerTodasAsEncomendas();
        this.BaseDeDados.CommitTransacao();
    }

    public void RegistarOrdemProducaoComoVisualizada(int identificador)
    {
        this.BaseDeDados.IniciarTransacao();
        OrdemProducaoModel? model = this.OrdensProducao.Obter(identificador);

        if (model == null)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new OrdemProducaoInexistenteException();
        }

        this.OrdensProducao.Atualizar(model with { Visualizada = true });
        this.BaseDeDados.CommitTransacao();
    }

    private void TentarSatisfazerEncomenda(EncomendaEVAs encomenda)
    {
        this.BaseDeDados.IniciarTransacao();

        bool podeSatisfazer = encomenda.Conteudo.All(entrada => entrada.Key.QuantidadeArmazem >= entrada.Value);

        if (podeSatisfazer)
        {
            foreach (EVA eva in encomenda.Conteudo.Keys)
            {
                eva.QuantidadeArmazem -= encomenda.ConteudoRaw[eva.Identificador];
                this.EVAs.Atualizar(eva.ParaModel());
            }

            encomenda.InstanteEntrega = DateTime.Now;
        }

        this.BaseDeDados.CommitTransacao();
    }

    private void TentarSatisfazerTodasAsEncomendas()
    {
        this.BaseDeDados.IniciarTransacao();

        List<EncomendaEVAs> encomendas = this.ObterTodasAsEncomendasEVAs();
        encomendas.Sort((e1, e2) => e1.InstanteColocacao.CompareTo(e2.InstanteColocacao));

        foreach (EncomendaEVAs encomenda in encomendas)
        {
            if (encomenda.Estado == EncomendaEVAs.EstadoEncomenda.Aprovada)
            {
                this.TentarSatisfazerEncomenda(encomenda);
                this.Encomendas.Atualizar(encomenda.ParaModel());
            }
        }

        this.BaseDeDados.CommitTransacao();
    }

    private IBaseDeDados BaseDeDados;
    private ICarrinhoComprasRepository CarrinhosCompras;
    private IEncomendaEVAsRepository Encomendas;
    private IEVARepository EVAs;
    private IParteRepository Partes;
    private IOrdemProducaoRepository OrdensProducao;
}
