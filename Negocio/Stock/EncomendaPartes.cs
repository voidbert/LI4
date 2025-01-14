using LI4.Dados;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Stock;

public class EncomendaPartes
{
    public EncomendaPartes(DateTime instanteRealizacao, Utilizador funcionario)
    {
        this.InstanteRealizacao = instanteRealizacao;
        this.Preco = 0.0;
        this._Funcionario = funcionario.EnderecoEletronico;
        this._Conteudo = new Dictionary<int, int>();
    }

    public EncomendaPartes(int identificador, DateTime instanteRealizacao, Utilizador funcionario, Dictionary<int, int> conteudo)
    {
        this.Identificador = identificador;
        this.InstanteRealizacao = instanteRealizacao;
        this.Preco = 0.0;
        this._Funcionario = funcionario.EnderecoEletronico;
        this._Conteudo = conteudo;
    }

    private EncomendaPartes(int? identificador, DateTime instanteRealizacao, double preco, string funcionario, Dictionary<int, int> conteudo)
    {
        this.Identificador = identificador;
        this.InstanteRealizacao = instanteRealizacao;
        this.Preco = preco;
        this._Funcionario = funcionario;
        this._Conteudo = conteudo;
    }

    public static EncomendaPartes DeModel(EncomendaPartesModel model)
    {
        return new EncomendaPartes(model.Identificador, model.InstanteRealizacao, model.Preco, model.Funcionario, model.Conteudo);
    }

    public void DefinirQuantidadeDeProduto(Parte parte, int quantidade)
    {
        int antiga = this._Conteudo.GetValueOrDefault(parte.Identificador);
        this.Preco -= antiga * parte.Preco;
        this.Preco += quantidade * parte.Preco;
        this._Conteudo[parte.Identificador] = quantidade;
    }

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public int? Identificador { get; init; }
    public DateTime InstanteRealizacao { get; set; }
    public double Preco { get; set; }
    private string _Funcionario;
    private Dictionary<int, int> _Conteudo { get; set; }

    public Task<Utilizador> Funcionario
    {
        get
        {
            return (UtilizadoresRepository.Instancia.Obter(this._Funcionario)!).ContinueWith(model => Utilizador.DeModel(model.Result!));
        }

        set
        {
            this._Funcionario = value.Result.EnderecoEletronico;
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
