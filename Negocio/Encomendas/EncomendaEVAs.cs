using LI4.Dados;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Encomendas;

public class EncomendaEVAs
{
    public enum EstadoEncomenda
    {
        Colocada,
        Rejeitada,
        Aprovada,
        Cancelada,
        Entregue,
        Devolvida
    }

    public EncomendaEVAs(string cliente, string morada, double preco, DateTime instanteColocacao, Dictionary<int, int> conteudo)
    {
        this.Identificador = null;
        this.ClienteRaw = cliente;
        this.Morada = morada;
        this.Preco = preco;
        this.InstanteColocacao = instanteColocacao;
        this.InstanteEntrega = null;
        this.InstanteCancelamento = null;
        this.InstanteDevolucao = null;
        this.Aprovada = false;
        this._ConteudoRaw = conteudo.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
    }

    private EncomendaEVAs(int identificador, string cliente, string morada, double preco, DateTime instanteColocacao, DateTime? instanteConfirmacao, DateTime? instanteEntrega, DateTime? instanteCancelamento, DateTime? instanteDevolucao, bool aprovada, Dictionary<int, int> conteudo)
    {
        this.Identificador = identificador;
        this.ClienteRaw = cliente;
        this.Morada = morada;
        this.Preco = preco;
        this.InstanteColocacao = instanteColocacao;
        this.InstanteConfirmacao = instanteConfirmacao;
        this.InstanteEntrega = instanteEntrega;
        this.InstanteCancelamento = instanteCancelamento;
        this.InstanteDevolucao = instanteDevolucao;
        this.Aprovada = aprovada;
        this._ConteudoRaw = conteudo;
    }

    public static EncomendaEVAs DeModel(EncomendaEVAsModel model)
    {
        return new EncomendaEVAs(model.Identificador!.Value, model.Cliente, model.Morada, model.Preco, model.InstanteColocacao, model.InstanteConfirmacao, model.InstanteEntrega, model.InstanteCancelamento, model.InstanteDevolucao, model.Aprovada, model.Conteudo);
    }

    public EncomendaEVAsModel ParaModel()
    {
        return new EncomendaEVAsModel
        {
            Identificador = this.Identificador,
            Cliente = this.ClienteRaw,
            Morada = this.Morada,
            Preco = this.Preco,
            InstanteColocacao = this.InstanteColocacao,
            InstanteConfirmacao = this.InstanteConfirmacao,
            InstanteEntrega = this.InstanteEntrega,
            InstanteCancelamento = this.InstanteCancelamento,
            InstanteDevolucao = this.InstanteDevolucao,
            Aprovada = this.Aprovada,
            Conteudo = this.ConteudoRaw
        };
    }

    public void DefinirQuantidadeDeEVA(EVA eva, int quantidade)
    {
        int antiga = this._ConteudoRaw.GetValueOrDefault(eva.Identificador);
        this.Preco -= antiga * eva.Preco;
        this.Preco += quantidade * eva.Preco;

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

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public int? Identificador { get; init; }
    public string ClienteRaw { get; set; }
    public string Morada { get; set; }
    public double Preco { get; set; }
    public DateTime InstanteColocacao { get; set; }
    public DateTime? InstanteConfirmacao { get; set; }
    public DateTime? InstanteEntrega { get; set; }
    public DateTime? InstanteCancelamento { get; set; }
    public DateTime? InstanteDevolucao { get; set; }
    public bool Aprovada { get; set; }
    private Dictionary<int, int> _ConteudoRaw;

    public Cliente Cliente
    {
        get
        {
            return (Cliente)Utilizador.DeModel(UtilizadorRepository.Instancia.Obter(this.ClienteRaw)!);
        }
        set
        {
            this.ClienteRaw = value.EnderecoEletronico;
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

    public EstadoEncomenda Estado
    {
        get
        {
            if (this.InstanteDevolucao != null)
            {
                return EstadoEncomenda.Devolvida;
            }
            else if (this.InstanteEntrega != null)
            {
                return EstadoEncomenda.Entregue;
            }
            else if (this.InstanteCancelamento != null)
            {
                return EstadoEncomenda.Cancelada;
            }
            else if (this.InstanteConfirmacao != null)
            {
                if (this.Aprovada)
                {
                    return EstadoEncomenda.Aprovada;
                }
                else
                {
                    return EstadoEncomenda.Rejeitada;
                }
            }
            else
            {
                return EstadoEncomenda.Colocada;
            }
        }
    }
}
