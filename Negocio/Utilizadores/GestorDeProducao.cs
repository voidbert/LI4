using LI4.Dados;
using LI4.Negocio.Producao;

namespace LI4.Negocio.Utilizadores;

public class GestorDeProducao : Utilizador
{
    public GestorDeProducao(string enderecoEletronico, string nomeCivil, byte[] palavraPasse, bool possivelIniciarSessao, List<int> ordensProducao)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao)
    {
        this._OrdensProducaoRaw = ordensProducao.ToList();
    }

    public GestorDeProducao(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao, List<int> ordensProducao)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao)
    {

        this._OrdensProducaoRaw = ordensProducao.ToList();
    }

    private List<int> _OrdensProducaoRaw { get; set; }

    public override Tipo TipoDeConta
    {
        get
        {
            return Tipo.GestorDeProducao;
        }
    }

    public List<int> OrdensProducaoRaw
    {
        get
        {
            return this._OrdensProducaoRaw.ToList();
        }
        set
        {
            this._OrdensProducaoRaw = value.ToList();
        }
    }

    public List<OrdemProducao> OrdensProducao
    {
        get
        {
            return this._OrdensProducaoRaw.Select(e => OrdemProducao.DeModel(OrdemProducaoRepository.Instancia.Obter(e)!)).ToList();
        }
        set
        {
            this._OrdensProducaoRaw = value.Select(e => e.Identificador!.Value).ToList();
        }
    }
}
