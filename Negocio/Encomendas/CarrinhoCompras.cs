using LI4.Dados;
using LI4.Negocio;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Encomendas;

public class CarrinhoCompras
{
    private CarrinhoCompras(string cliente, Dictionary<int, int> conteudo)
    {
        this._Cliente = cliente;
        this._Conteudo = conteudo;
    }

    public static CarrinhoCompras DeModel(CarrinhoComprasModel model)
    {
        string.Join(Environment.NewLine, model.Conteudo);
        return new CarrinhoCompras(model.Cliente, model.Conteudo);
    }

    private string _Cliente;
    private Dictionary<int, int> _Conteudo;

    public void DefinirQuantidadeDeEVA(EVA eva, int quantidade)
    {
        if (quantidade == 0)
        {
            if (this._Conteudo.ContainsKey(eva.Identificador))
            {
                this._Conteudo.Remove(eva.Identificador);
            }
        }
        else
        {
            this._Conteudo[eva.Identificador] = quantidade;
        }
    }

    public async Task<double> CalcularPreco()
    {
        double preco = 0.0;

        foreach (KeyValuePair<int, int> entrada in _Conteudo)
        {
            EVAModel? evaModel = await EVARepository.Instancia.Obter(entrada.Key)!;
            EVA eva = EVA.DeModel(evaModel!);
            preco += eva.Preco * entrada.Value;
        }

        return preco;
    }

    public Task<Utilizador> Cliente
    {
        get
        {
            return (UtilizadoresRepository.Instancia.Obter(this._Cliente)!).ContinueWith(model => Utilizador.DeModel(model.Result!));
        }
    }

    public Dictionary<int, int> Conteudo
    {
        get
        {
            return this._Conteudo.ToDictionary(entry => entry.Key, entry => entry.Value);
        }
        set
        {
            this._Conteudo = value.ToDictionary(entry => entry.Key, entry => entry.Value);
        }
    }
}
