using LI4.Dados;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Producao;

public class OrdemProducao
{
    public OrdemProducao(DateTime instanteEmissao, Utilizador funcionario)
    {
        this.Identificador = null;
        this._Funcionario = funcionario.EnderecoEletronico;
        this.InstanteEmissao = instanteEmissao;
        this.Visualizada = false;
        this._Conteudo = new Dictionary<int, int>();
    }

    private OrdemProducao(int? identificador, string funcionario, DateTime instanteEmissao, bool visualizada, Dictionary<int, int> conteudo)
    {
        this.Identificador = identificador;
        this._Funcionario = funcionario;
        this.InstanteEmissao = instanteEmissao;
        this.Visualizada = visualizada;
        this._Conteudo = conteudo;
    }

    public static OrdemProducao DeModel(OrdemProducaoModel model)
    {
        return new OrdemProducao(model.Identificador, model.Funcionario, model.InstanteEmissao, model.Visualizada, model.Conteudo);
    }

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public void DefinirQuantidadeDeEVA(EVA eva, int quantidade)
    {
        if (this._Conteudo.ContainsKey(eva.Identificador) && quantidade == 0)
        {
            this._Conteudo.Remove(eva.Identificador);
        }
        else
        {
            this._Conteudo[eva.Identificador] = quantidade;
        }
    }

    public int? Identificador { get; init; }
    private string _Funcionario;
    public DateTime InstanteEmissao { get; set; }
    public bool Visualizada { get; set; }
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
