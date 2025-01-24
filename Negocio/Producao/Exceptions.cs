namespace LI4.Negocio.Producao;

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

[Serializable]
public class OrdemProducaoVaziaException : Exception
{
    public OrdemProducaoVaziaException() { }
    public OrdemProducaoVaziaException(string message) : base(message) { }
    public OrdemProducaoVaziaException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class SemPartesException : Exception
{
    public SemPartesException() { }
    public SemPartesException(string message) : base(message) { }
    public SemPartesException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class OrdemProducaoInexistenteException : Exception
{
    public OrdemProducaoInexistenteException() { }
    public OrdemProducaoInexistenteException(string message) : base(message) { }
    public OrdemProducaoInexistenteException(string message, Exception innerException) : base(message, innerException) { }
}
