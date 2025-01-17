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
}
