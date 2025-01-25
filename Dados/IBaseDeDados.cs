namespace LI4.Dados;

public interface IBaseDeDados
{
    public List<T> LerDados<T, U>(string sql, U parametros);
    public void EscreverDados<T>(string sql, T parametros);
    public void IniciarTransacao();
    public void CommitTransacao();
    public void AbortarTransacao();
}
