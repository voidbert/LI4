namespace LI4.Dados;

public record CarrinhoComprasModel
{
    public required string Cliente { get; init; }
    public required Dictionary<int, int> Conteudo { get; set; }
}
