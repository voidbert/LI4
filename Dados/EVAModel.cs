namespace LI4.Dados;

public record EVAModel
{
    public required int Identificador { get; init; }
    public required string Nome { get; set; }
    public required string Imagem { get; set; }
    public required double Preco { get; set; }
    public required int QuantidadeArmazem { get; set; }
    public required Dictionary<int, int> Partes { get; set; }
}
