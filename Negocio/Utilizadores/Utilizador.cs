using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

using LI4.Dados;

namespace LI4.Negocio.Utilizadores;

public abstract class Utilizador
{
    public enum Tipo
    {
        Cliente,
        Administrador,
        GestorDeStock,
        GestorDeProducao,
        GestorDeContas
    }

    protected Utilizador(string enderecoEletronico, string nomeCivil, byte[] palavraPasse, bool possivelIniciarSessao)
    {
        this.EnderecoEletronico = enderecoEletronico;
        this.NomeCivil = nomeCivil;
        this.PalavraPasse = palavraPasse;
        this.PossivelIniciarSessao = possivelIniciarSessao;
    }

    protected Utilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao)
    {
        this.EnderecoEletronico = enderecoEletronico;
        this.NomeCivil = nomeCivil;
        this.PalavraPasse = Utilizador.HashDaPalavraPasse(palavraPasse);
        this.PossivelIniciarSessao = possivelIniciarSessao;
    }

    public static byte[] HashDaPalavraPasse(string palavraPasse)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(palavraPasse);
        return SHA512.Create().ComputeHash(bytes);
    }

    public static Tipo TipoDeString(string tipo)
    {
        Dictionary<string, Tipo> strings = new Dictionary<string, Tipo> {
            {"C", Tipo.Cliente},
            {"A", Tipo.Administrador},
            {"GS", Tipo.GestorDeStock},
            {"GP", Tipo.GestorDeProducao},
            {"GC", Tipo.GestorDeContas}
        };

        return strings[tipo];
    }

    public static string StringDeTipo(Tipo tipo)
    {
        Dictionary<Tipo, string> strings = new Dictionary<Tipo, string> {
            {Tipo.Cliente, "C"},
            {Tipo.Administrador, "A"},
            {Tipo.GestorDeStock, "GS"},
            {Tipo.GestorDeProducao, "GP"},
            {Tipo.GestorDeContas, "GC"}
        };

        return strings[tipo];
    }

    public static Utilizador DeModel(UtilizadorModel model)
    {
        Tipo tipo = Utilizador.TipoDeString(model.TipoDeConta);
        switch (tipo)
        {
            case Tipo.Cliente:
                return new Cliente(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao);
            case Tipo.Administrador:
                return new Administrador(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao);
            case Tipo.GestorDeStock:
                return new GestorDeStock(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao);
            case Tipo.GestorDeProducao:
                return new GestorDeProducao(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao);
            case Tipo.GestorDeContas:
            default:
                return new GestorDeContas(model.EnderecoEletronico, model.NomeCivil, model.PalavraPasse, model.PossivelIniciarSessao);
        }
    }

    public string EnderecoEletronico { get; set; }
    public string NomeCivil { get; set; }
    public byte[] PalavraPasse { get; set; }
    public abstract Tipo TipoDeConta { get; }
    public bool PossivelIniciarSessao { get; set; }
}
