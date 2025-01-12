namespace LI4.Negocio.Utilizadores;

public class GestorDeProducao : Utilizador
{
    public GestorDeProducao(string enderecoEletronico, string nomeCivil, byte[] palavraPasse, bool possivelIniciarSessao)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao) { }

    public GestorDeProducao(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao) { }

    public override Tipo TipoDeConta
    {
        get
        {
            return Tipo.GestorDeProducao;
        }
    }
}
