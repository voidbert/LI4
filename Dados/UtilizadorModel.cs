namespace LI4.Dados;

public record UtilizadorModel
{
    public required string EnderecoEletronico { get; init; }
    public required string NomeCivil { get; set; }
    public required byte[] PalavraPasse { get; set; }
    public required string TipoDeConta { get; set; }
    public required bool PossivelIniciarSessao { get; set; }
}
