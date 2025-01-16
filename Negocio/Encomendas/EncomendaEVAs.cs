using LI4.Dados;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Encomendas;

public class EncomendaEVAs
{
    public EncomendaEVAs(Utilizador cliente, string morada, double preco, DateTime instanteColocacao, Dictionary<int, int> conteudo)
    {
        this._Cliente = cliente.EnderecoEletronico;
        this.Morada = morada;
        this.Preco = preco;
        this.InstanteColocacao = instanteColocacao;
        this._Conteudo = conteudo;
    }

    private EncomendaEVAs(int identificador, string cliente, string morada, double preco, DateTime instanteColocacao, DateTime? instanteConfirmacao, DateTime? instanteEntrega, DateTime? instanteCancelamento, DateTime? instanteDevolucao, bool aprovada, Dictionary<int, int> conteudo)
    {
        this.Identificador = identificador;
        this._Cliente = cliente;
        this.Morada = morada;
        this.Preco = preco;
        this.InstanteColocacao = instanteColocacao;
        this.InstanteConfirmacao = instanteConfirmacao;
        this.InstanteEntrega = instanteEntrega;
        this.InstanteCancelamento = instanteCancelamento;
        this.InstanteDevolucao = instanteDevolucao;
        this._Conteudo = conteudo;
    }

    public static EncomendaEVAs DeModel(EncomendaEVAsModel model)
    {
        return new EncomendaEVAs(model.Identificador!.Value, model.Cliente, model.Morada, model.Preco, model.InstanteColocacao, model.InstanteConfirmacao, model.InstanteEntrega, model.InstanteCancelamento, model.InstanteDevolucao, model.Aprovada, model.Conteudo);
    }

    public void DefinirQuantidadeDeEVA(EVA eva, int quantidade)
    {
        int antiga = this._Conteudo.GetValueOrDefault(eva.Identificador);
        this.Preco -= antiga * eva.Preco;
        this.Preco += quantidade * eva.Preco;

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

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public int? Identificador { get; init; }
    public string _Cliente { get; set; }
    public string Morada { get; set; }
    public double Preco { get; set; }
    public DateTime InstanteColocacao { get; set; }
    public DateTime? InstanteConfirmacao { get; set; }
    public DateTime? InstanteEntrega { get; set; }
    public DateTime? InstanteCancelamento { get; set; }
    public DateTime? InstanteDevolucao { get; set; }
    public bool Aprovada { get; set; }
    public Dictionary<int, int> _Conteudo { get; set; }

    public Task<Utilizador> Cliente
    {
        get
        {
            return (UtilizadoresRepository.Instancia.Obter(this._Cliente)!).ContinueWith(model => Utilizador.DeModel(model.Result!));
        }

        set
        {
            this._Cliente = value.Result.EnderecoEletronico;
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
