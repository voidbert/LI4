using LI4.Dados;

namespace LI4.Negocio.Encomendas;

public class EncomendasService
{
    public static async Task<CarrinhoCompras> ObterCarrinho(string cliente)
    {
        CarrinhoComprasModel model = await CarrinhoComprasRepository.Instancia.Obter(cliente);
        return CarrinhoCompras.DeModel(model);
    }

    public async Task<List<EncomendaEVAs>> ObterTodasAsEncomendasEVAs()
    {
        List<EncomendaEVAsModel> modelos = await EncomendaEVAsRepository.Instancia.ObterTodas();
        return modelos.Select(model => EncomendaEVAs.DeModel(model)).ToList();
    }

    public static async Task AtualizarCarrinho(CarrinhoCompras carrinho)
    {
        CarrinhoComprasModel model = new CarrinhoComprasModel
        {
            Cliente = (await carrinho.Cliente).EnderecoEletronico,
            Conteudo = carrinho.Conteudo
        };

        await CarrinhoComprasRepository.Instancia.Atualizar(model);
    }

    public async Task ColocarEncomenda(EncomendaEVAs encomenda)
    {
        if (encomenda.Conteudo.Count == 0)
        {
            throw new CarrinhoVazioException();
        }

        EncomendaEVAsModel model = new EncomendaEVAsModel
        {
            Cliente = (await encomenda.Cliente).EnderecoEletronico,
            Morada = encomenda.Morada,
            Preco = encomenda.Preco,
            InstanteColocacao = encomenda.InstanteColocacao,
            InstanteConfirmacao = encomenda.InstanteConfirmacao,
            InstanteEntrega = encomenda.InstanteEntrega,
            InstanteCancelamento = encomenda.InstanteCancelamento,
            InstanteDevolucao = encomenda.InstanteDevolucao,
            Aprovada = encomenda.Aprovada,
            Conteudo = encomenda.Conteudo
        };
        await EncomendaEVAsRepository.Instancia.Adicionar(model);
    }
}
