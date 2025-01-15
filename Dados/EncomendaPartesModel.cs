namespace LI4.Dados;

public record EncomendaPartesModel
{
    public int? Identificador { get; init; }
    public required DateTime InstanteRealizacao { get; set; }
    public required double Preco { get; set; }
    public required string Funcionario { get; set; }
    public required Dictionary<int, int> Conteudo { get; set; }
}
