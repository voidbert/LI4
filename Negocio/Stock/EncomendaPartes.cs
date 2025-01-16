using LI4.Dados;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Stock;

public class EncomendaPartes
{
    public EncomendaPartes(DateTime instanteRealizacao, string funcionario)
    {
        this.Identificador = null;
        this.InstanteRealizacao = instanteRealizacao;
        this.Preco = 0.0;
        this.FuncionarioRaw = funcionario;
        this._ConteudoRaw = new Dictionary<int, int>();
    }

    private EncomendaPartes(int? identificador, DateTime instanteRealizacao, double preco, string funcionario, Dictionary<int, int> conteudo)
    {
        this.Identificador = identificador;
        this.InstanteRealizacao = instanteRealizacao;
        this.Preco = preco;
        this.FuncionarioRaw = funcionario;
        this._ConteudoRaw = conteudo.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
    }

    public static EncomendaPartes DeModel(EncomendaPartesModel model)
    {
        return new EncomendaPartes(model.Identificador, model.InstanteRealizacao, model.Preco, model.Funcionario, model.Conteudo);
    }

    public EncomendaPartesModel ParaModel()
    {
        return new EncomendaPartesModel
        {
            Identificador = this.Identificador,
            InstanteRealizacao = this.InstanteRealizacao,
            Preco = this.Preco,
            Funcionario = this.FuncionarioRaw,
            Conteudo = this.ConteudoRaw
        };
    }

    public void DefinirQuantidadeDeParte(Parte parte, int quantidade)
    {
        int antiga = this._ConteudoRaw.GetValueOrDefault(parte.Identificador);
        this.Preco -= antiga * parte.Preco;
        this.Preco += quantidade * parte.Preco;

        if (quantidade == 0)
        {
            if (this._ConteudoRaw.ContainsKey(parte.Identificador))
            {
                this._ConteudoRaw.Remove(parte.Identificador);
            }
        }
        else
        {
            this._ConteudoRaw[parte.Identificador] = quantidade;
        }
    }

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public int? Identificador { get; init; }
    public DateTime InstanteRealizacao { get; set; }
    public double Preco { get; set; }
    public string FuncionarioRaw { get; set; }
    private Dictionary<int, int> _ConteudoRaw { get; set; }

    public Utilizador Funcionario
    {
        get
        {
            return Utilizador.DeModel(UtilizadorRepository.Instancia.Obter(this.FuncionarioRaw)!);
        }
        set
        {
            this.FuncionarioRaw = value.EnderecoEletronico;
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

    public Dictionary<Parte, int> Conteudo
    {
        get
        {
            return this._ConteudoRaw.ToDictionary(entrada => Parte.DeModel(ParteRepository.Instancia.Obter(entrada.Key)!), entrada => entrada.Value);
        }
        set
        {
            this._ConteudoRaw = value.ToDictionary(entrada => entrada.Key.Identificador, entrada => entrada.Value);
        }
    }
}
