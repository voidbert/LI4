namespace LI4.Negocio.Producao;

public interface IGestaoProducao
{
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
}
