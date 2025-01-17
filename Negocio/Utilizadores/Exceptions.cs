namespace LI4.Negocio.Utilizadores;

[Serializable]
public class TipoDeContaInexistenteException : Exception
{
    public TipoDeContaInexistenteException() { }
    public TipoDeContaInexistenteException(string message) : base(message) { }
    public TipoDeContaInexistenteException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class UtilizadorNaoEncontradoException : Exception
{
    public UtilizadorNaoEncontradoException() { }
    public UtilizadorNaoEncontradoException(string message) : base(message) { }
    public UtilizadorNaoEncontradoException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class PalavraPasseIncorretaException : Exception
{
    public PalavraPasseIncorretaException() { }
    public PalavraPasseIncorretaException(string message) : base(message) { }
    public PalavraPasseIncorretaException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class ImpedidoDeIniciarSessaoException : Exception
{
    public ImpedidoDeIniciarSessaoException() { }
    public ImpedidoDeIniciarSessaoException(string message) : base(message) { }
    public ImpedidoDeIniciarSessaoException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class EnderecoEletronicoInvalidoException : Exception
{
    public EnderecoEletronicoInvalidoException() { }
    public EnderecoEletronicoInvalidoException(string message) : base(message) { }
    public EnderecoEletronicoInvalidoException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class UtilizadorExistenteException : Exception
{
    public UtilizadorExistenteException() { }
    public UtilizadorExistenteException(string message) : base(message) { }
    public UtilizadorExistenteException(string message, Exception innerException) : base(message, innerException) { }
}
