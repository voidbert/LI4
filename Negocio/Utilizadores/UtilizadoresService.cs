using System.Net.Mail;

using LI4.Dados;

namespace LI4.Negocio.Utilizadores;

public class UtilizadoresService
{
    public async Task<Utilizador> IniciarSessao(string enderecoEletronico, string palavraPasse)
    {
        UtilizadorModel? utilizadorModel = await UtilizadoresRepository.Instance.Get(enderecoEletronico);
        if (utilizadorModel == null)
        {
            throw new UtilizadorNaoEncontradoException();
        }

        Utilizador? utilizador = Utilizador.DeModel(utilizadorModel);

        byte[] hash = Utilizador.HashDaPalavraPasse(palavraPasse);
        if (!Enumerable.SequenceEqual(hash, utilizador.PalavraPasse))
        {
            throw new PalavraPasseIncorretaException();
        }

        if (!utilizador.PossivelIniciarSessao)
        {
            throw new ImpedidoDeIniciarSessaoException();
        }

        return utilizador;
    }

    public async Task RegistarUtilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, Utilizador.Tipo tipoDeConta)
    {
        try
        {
            new MailAddress(enderecoEletronico);
        }
        catch (FormatException)
        {
            throw new EnderecoEletronicoInvalidoException();
        }

        UtilizadorModel? existente = await UtilizadoresRepository.Instance.Get(enderecoEletronico);
        if (existente != null)
        {
            throw new EnderecoEletronicoExistenteException();
        }

        byte[] hash = Utilizador.HashDaPalavraPasse(palavraPasse);
        String tipoString = Utilizador.StringDeTipo(tipoDeConta);
        UtilizadorModel model = new UtilizadorModel
        {
            EnderecoEletronico = enderecoEletronico,
            NomeCivil = nomeCivil,
            PalavraPasse = hash,
            TipoDeConta = tipoString,
            PossivelIniciarSessao = true
        };

        await UtilizadoresRepository.Instance.Add(model);
    }
}
