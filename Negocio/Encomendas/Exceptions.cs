namespace LI4.Negocio.Encomendas;

[Serializable]
public class CarrinhoVazioException : Exception
{
    public CarrinhoVazioException() { }
    public CarrinhoVazioException(string message) : base(message) { }
    public CarrinhoVazioException(string message, Exception innerException) : base(message, innerException) { }
}
