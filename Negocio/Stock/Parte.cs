using LI4.Dados;

namespace LI4.Negocio.Stock;

public class Parte
{
    public Parte(int identificador, string nome, double preco, int quantidadeArmazem)
    {
        this.Identificador = identificador;
        this.Nome = nome;
        this.Preco = preco;
        this.QuantidadeArmazem = quantidadeArmazem;
    }

    public static Parte DeModel(ParteModel model)
    {
        return new Parte(model.Identificador, model.Nome, model.Preco, model.QuantidadeArmazem);
    }

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public int Identificador { get; init; }
    public string Nome { get; set; }
    public double Preco { get; set; }
    public int QuantidadeArmazem { get; }
}
