namespace LI4.Negocio.Stock;

[Serializable]
public class ParteNaoEncontradaException : Exception
{
    public ParteNaoEncontradaException() { }
    public ParteNaoEncontradaException(string message) : base(message) { }
    public ParteNaoEncontradaException(string message, Exception innerException) : base(message, innerException) { }
}
