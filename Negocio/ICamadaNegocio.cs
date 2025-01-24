using LI4.Negocio.Producao;
using LI4.Negocio.Stock;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio;

public interface ICamadaNegocio
{
    public List<EVA> ObterTodasAsEVAs();

    public CarrinhoCompras ObterCarrinho(string cliente);
    public List<EncomendaEVAs> ObterTodasAsEncomendasEVAs();
    public void AtualizarCarrinho(CarrinhoCompras carrinho);
    public void ColocarEncomendaEVAs(EncomendaEVAs encomenda);
    public void RejeitarEncomendaEVAs(int identificador);
    public void AprovarEncomendaEVAs(int identificador);
    public void CancelarEncomendaEVAs(int identificador);
    public void DevolverEncomendaEVAs(int identificador);
    public void ColocarOrdemProducao(OrdemProducao ordemProducao);
    public void RegistarOrdemProducaoComoVisualizada(int identificador);

    public List<Parte> ObterTodasAsPartes();
    public List<EncomendaPartes> ObterTodasAsEncomendasPartes();
    public void ColocarEncomendaPartes(EncomendaPartes encomenda);

    public List<Utilizador> ObterTodosOsUtilizadores();
    public Utilizador IniciarSessao(string enderecoEletronico, string palavraPasse);
    public void RegistarUtilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, Utilizador.Tipo tipoDeConta);
    public void RegistarComoImpedidoDeIniciarSessao(string enderecoEletronico);
}
