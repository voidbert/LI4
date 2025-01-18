namespace LI4.Negocio.Producao;

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
