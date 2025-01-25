using LI4.Dados;
using LI4.Negocio.Producao;
using LI4.Negocio.Stock;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio;

public class CamadaNegocioService : ICamadaNegocio
{
    public CamadaNegocioService()
    {
        this.EVAs = EVARepository.Instancia;
        this.Producao = new ProducaoService();
        this.Stock = new StockService();
        this.Utilizadores = new UtilizadoresService();
    }

    public List<EVA> ObterTodasAsEVAs()
    {
        return this.EVAs.ObterTodas().Select(model => EVA.DeModel(model)).ToList();
    }

    public CarrinhoCompras ObterCarrinho(string cliente)
    {
        return this.Producao.ObterCarrinho(cliente);
    }

    public List<EncomendaEVAs> ObterTodasAsEncomendasEVAs()
    {
        return this.Producao.ObterTodasAsEncomendasEVAs();
    }

    public void AtualizarCarrinho(CarrinhoCompras carrinho)
    {
        this.Producao.AtualizarCarrinho(carrinho);
    }

    public void ColocarEncomendaEVAs(EncomendaEVAs encomenda)
    {
        this.Producao.ColocarEncomendaEVAs(encomenda);
    }

    public void RejeitarEncomendaEVAs(int identificador)
    {
        this.Producao.RejeitarEncomendaEVAs(identificador);
    }

    public void AprovarEncomendaEVAs(int identificador)
    {
        this.Producao.AprovarEncomendaEVAs(identificador);
    }

    public void CancelarEncomendaEVAs(int identificador)
    {
        this.Producao.CancelarEncomendaEVAs(identificador);
    }

    public void DevolverEncomendaEVAs(int identificador)
    {
        this.Producao.DevolverEncomendaEVAs(identificador);
    }

    public void ColocarOrdemProducao(OrdemProducao ordemProducao)
    {
        this.Producao.ColocarOrdemProducao(ordemProducao);
    }

    public void RegistarOrdemProducaoComoVisualizada(int identificador)
    {
        this.Producao.RegistarOrdemProducaoComoVisualizada(identificador);
    }

    public List<Parte> ObterTodasAsPartes()
    {
        return this.Stock.ObterTodasAsPartes();
    }

    public List<EncomendaPartes> ObterTodasAsEncomendasPartes()
    {
        return this.Stock.ObterTodasAsEncomendasPartes();
    }

    public void ColocarEncomendaPartes(EncomendaPartes encomenda)
    {
        this.Stock.ColocarEncomendaPartes(encomenda);
    }

    public List<Utilizador> ObterTodosOsUtilizadores()
    {
        return this.Utilizadores.ObterTodosOsUtilizadores();
    }

    public Utilizador IniciarSessao(string enderecoEletronico, string palavraPasse)
    {
        return this.Utilizadores.IniciarSessao(enderecoEletronico, palavraPasse);
    }

    public void RegistarUtilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, Utilizador.Tipo tipoDeConta)
    {
        this.Utilizadores.RegistarUtilizador(enderecoEletronico, nomeCivil, palavraPasse, tipoDeConta);
    }

    public void RegistarComoImpedidoDeIniciarSessao(string enderecoEletronico)
    {
        this.Utilizadores.RegistarComoImpedidoDeIniciarSessao(enderecoEletronico);
    }

    private EVARepository EVAs;
    private ProducaoService Producao;
    private IGestaoStock Stock;
    private IGestaoUtilizadores Utilizadores;
}
