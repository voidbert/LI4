using LI4.Dados;
using LI4.Negocio;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Encomendas;

public class CarrinhoCompras
{
    private CarrinhoCompras(string cliente, Dictionary<int, int> conteudo)
    {
        this.ClienteRaw = cliente;
        this._ConteudoRaw = conteudo.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
    }

    public static CarrinhoCompras DeModel(CarrinhoComprasModel model)
    {
        return new CarrinhoCompras(model.Cliente, model.Conteudo);
    }

    public CarrinhoComprasModel ParaModel()
    {
        return new CarrinhoComprasModel
        {
            Cliente = this.ClienteRaw,
            Conteudo = this.ConteudoRaw
        };
    }

    public string ClienteRaw { get; set; }
    private Dictionary<int, int> _ConteudoRaw;

    public void DefinirQuantidadeDeEVA(EVA eva, int quantidade)
    {
        if (quantidade == 0)
        {
            if (this._ConteudoRaw.ContainsKey(eva.Identificador))
            {
                this._ConteudoRaw.Remove(eva.Identificador);
            }
        }
        else
        {
            this._ConteudoRaw[eva.Identificador] = quantidade;
        }
    }

    public double CalcularPreco()
    {
        double preco = 0.0;

        foreach (KeyValuePair<EVA, int> entrada in this.Conteudo)
        {
            preco += entrada.Key.Preco * entrada.Value;
        }

        return preco;
    }

    public Utilizador Cliente
    {
        get
        {
            return Utilizador.DeModel(UtilizadorRepository.Instancia.Obter(this.ClienteRaw)!);
        }
        set
        {
            this.ClienteRaw = Cliente.EnderecoEletronico;
        }
    }

    public Dictionary<int, int> ConteudoRaw
    {
        get
        {
            return this._ConteudoRaw.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
        }
        set
        {
            this._ConteudoRaw = value.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
        }
    }

    public Dictionary<EVA, int> Conteudo
    {
        get
        {
            return this._ConteudoRaw.ToDictionary(entrada => EVA.DeModel(EVARepository.Instancia.Obter(entrada.Key)!), entrada => entrada.Value);
        }
        set
        {
            this._ConteudoRaw = value.ToDictionary(entrada => entrada.Key.Identificador, entrada => entrada.Value);
        }
    }
}
