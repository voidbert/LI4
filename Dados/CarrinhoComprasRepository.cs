namespace LI4.Dados;

public class CarrinhoComprasRepository
{
    private static CarrinhoComprasRepository? _Instancia;

    private CarrinhoComprasRepository() { }

    public static CarrinhoComprasRepository Instancia
    {
        get
        {
            if (CarrinhoComprasRepository._Instancia == null)
                CarrinhoComprasRepository._Instancia = new CarrinhoComprasRepository();
            return CarrinhoComprasRepository._Instancia;
        }
    }

    public CarrinhoComprasModel Obter(string cliente)
    {
        string sql = "SELECT EVA AS [Key], Quantidade AS Value FROM CarrinhoCompras WHERE Cliente=@cliente";
        List<KeyValuePair<int, int>> lista = BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(sql, new
        {
            cliente = cliente
        });

        return new CarrinhoComprasModel
        {
            Cliente = cliente,
            Conteudo = new Dictionary<int, int>(lista)
        };
    }

    public void Atualizar(CarrinhoComprasModel carrinhoCompras)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string apagarSql = "DELETE FROM CarrinhoCompras WHERE Cliente=@cliente";
        BaseDeDados.Instancia.EscreverDados<dynamic>(apagarSql, new
        {
            cliente = carrinhoCompras.Cliente
        });

        string inserirSql = "INSERT INTO CarrinhoCompras (Cliente, EVA, Quantidade) VALUES (@cliente, @eva, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in carrinhoCompras.Conteudo)
        {
            BaseDeDados.Instancia.EscreverDados<dynamic>(inserirSql, new
            {
                cliente = carrinhoCompras.Cliente,
                eva = entrada.Key,
                quantidade = entrada.Value
            });
        }

        BaseDeDados.Instancia.CommitTransacao();
    }
}
