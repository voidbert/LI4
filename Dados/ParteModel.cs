namespace LI4.Dados;

public record ParteModel
{
    public required int Identificador { get; init; }
    public required string Nome { get; set; }
    public required double Preco { get; set; }
    public required int QuantidadeArmazem { get; set; }
}
