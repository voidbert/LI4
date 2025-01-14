using LI4.Dados;

namespace LI4.Negocio;

public class EVA
{
    public EVA(int identificador, string nome, string imagem, double preco, int quantidadeArmazem, Dictionary<int, int> partes)
    {
        this.Identificador = identificador;
        this.Nome = nome;
        this.Imagem = imagem;
        this.Preco = preco;
        this.QuantidadeArmazem = quantidadeArmazem;
        this._Partes = partes;
    }

    public static EVA DeModel(EVAModel model)
    {
        return new EVA(model.Identificador, model.Nome, model.Imagem, model.Preco, model.QuantidadeArmazem, model.Partes);
    }

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public int Identificador { get; init; }
    public string Nome { get; set; }
    public string Imagem { get; set; }
    public double Preco { get; set; }
    public int QuantidadeArmazem { get; }
    private Dictionary<int, int> _Partes;

    public Dictionary<int, int> Partes
    {
        get
        {
            return this._Partes.ToDictionary(entry => entry.Key, entry => entry.Value);
        }
        set
        {
            this._Partes = value.ToDictionary(entry => entry.Key, entry => entry.Value);
        }
    }
}
