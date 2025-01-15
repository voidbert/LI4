namespace LI4.Dados;

public record OrdemProducaoModel
{
    public int? Identificador { get; init; }
    public required string Funcionario { get; set; }
    public required DateTime InstanteEmissao { get; set; }
    public required bool Visualizada { get; set; }
    public required Dictionary<int, int> Conteudo { get; set; }
}
