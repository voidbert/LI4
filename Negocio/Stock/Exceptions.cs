namespace LI4.Negocio.Stock;

[Serializable]
public class EncomendaVaziaException : Exception
{
    public EncomendaVaziaException() { }
    public EncomendaVaziaException(string message) : base(message) { }
    public EncomendaVaziaException(string message, Exception innerException) : base(message, innerException) { }
}
