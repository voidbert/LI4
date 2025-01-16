namespace LI4.Dados;

public record EncomendaEVAsModel
{
    public int? Identificador { get; init; }
    public required string Cliente { get; set; }
    public required string Morada { get; set; }
    public required double Preco { get; set; }
    public required DateTime InstanteColocacao { get; set; }
    public DateTime? InstanteConfirmacao { get; set; }
    public DateTime? InstanteEntrega { get; set; }
    public DateTime? InstanteCancelamento { get; set; }
    public DateTime? InstanteDevolucao { get; set; }
    public required bool Aprovada { get; set; }
    public required Dictionary<int, int> Conteudo { get; set; }
}
