namespace LI4.Dados;

public interface ICarrinhoComprasRepository
{
    public CarrinhoComprasModel Obter(string cliente);
    public void Atualizar(CarrinhoComprasModel carrinhoCompras);
}
