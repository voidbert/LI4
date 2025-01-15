using LI4.Dados;

namespace LI4.Negocio.Encomendas;

public class EncomendasService
{
    public async Task<CarrinhoCompras> ObterCarrinho(string cliente)
    {
        CarrinhoComprasModel model = await CarrinhoComprasRepository.Instancia.Obter(cliente);
        return CarrinhoCompras.DeModel(model);
    }

    public async Task AtualizarCarrinho(CarrinhoCompras carrinho)
    {
        CarrinhoComprasModel model = new CarrinhoComprasModel
        {
            Cliente = (await carrinho.Cliente).EnderecoEletronico,
            Conteudo = carrinho.Conteudo
        };

        await CarrinhoComprasRepository.Instancia.Atualizar(model);
    }
}
