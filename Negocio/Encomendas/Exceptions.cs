namespace LI4.Negocio.Encomendas;

[Serializable]
public class CarrinhoVazioException : Exception
{
    public CarrinhoVazioException() { }
    public CarrinhoVazioException(string message) : base(message) { }
    public CarrinhoVazioException(string message, Exception innerException) : base(message, innerException) { }
}


[Serializable]
public class EncomendaNaoEncontradaException : Exception
{
    public EncomendaNaoEncontradaException() { }
    public EncomendaNaoEncontradaException(string message) : base(message) { }
    public EncomendaNaoEncontradaException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class EstadoInvalidoException : Exception
{
    public EstadoInvalidoException() { }
    public EstadoInvalidoException(string message) : base(message) { }
    public EstadoInvalidoException(string message, Exception innerException) : base(message, innerException) { }
}
