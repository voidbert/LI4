using System.Net.Mail;

using LI4.Dados;

namespace LI4.Negocio.Utilizadores;

public class UtilizadoresService
{
    public async Task<Utilizador> IniciarSessao(string enderecoEletronico, string palavraPasse)
    {
        UtilizadorModel? utilizadorModel = await UtilizadoresRepository.Instancia.Obter(enderecoEletronico);
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

        BaseDeDados.Instancia.IniciarTransacao();

        UtilizadorModel? existente = await UtilizadoresRepository.Instancia.Obter(enderecoEletronico);
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

        await UtilizadoresRepository.Instancia.Adicionar(model);
        BaseDeDados.Instancia.CommitTransacao();
    }

    public async Task<List<Utilizador>> ObterTodos()
    {
        List<UtilizadorModel> modelos = await UtilizadoresRepository.Instancia.ObterTodos();
        return modelos.Select(model => Utilizador.DeModel(model)).ToList();
    }

    public async Task RegistarComoImpedidoDeIniciarSessao(string enderecoEletronico)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        UtilizadorModel? model = await UtilizadoresRepository.Instancia.Obter(enderecoEletronico);
        if (model == null)
        {
            throw new UtilizadorNaoEncontradoException();
        }

        model.PossivelIniciarSessao = false;
        await UtilizadoresRepository.Instancia.Atualizar(model);

        BaseDeDados.Instancia.CommitTransacao();
    }
}
