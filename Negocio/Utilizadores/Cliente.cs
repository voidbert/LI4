using LI4.Dados;
using LI4.Negocio.Encomendas;

namespace LI4.Negocio.Utilizadores;

public class Cliente : Utilizador
{
    public Cliente(string enderecoEletronico, string nomeCivil, byte[] palavraPasse, bool possivelIniciarSessao, List<int> encomendas)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao)
    {
        this._EncomendasRaw = encomendas.ToList();
    }

    public Cliente(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao, List<int> encomendas)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao)
    {
        this._EncomendasRaw = encomendas.ToList();
    }

    private List<int> _EncomendasRaw { get; set; }

    public override Tipo TipoDeConta
    {
        get
        {
            return Tipo.Cliente;
        }
    }

    public List<int> EncomendasRaw
    {
        get
        {
            return this._EncomendasRaw.ToList();
        }
        set
        {
            this._EncomendasRaw = value.ToList();
        }
    }

    public List<EncomendaEVAs> Encomendas
    {
        get
        {
            return this._EncomendasRaw.Select(e => EncomendaEVAs.DeModel(EncomendaEVAsRepository.Instancia.Obter(e)!)).ToList();
        }
        set
        {
            this._EncomendasRaw = value.Select(e => e.Identificador!.Value).ToList();
        }
    }
}
