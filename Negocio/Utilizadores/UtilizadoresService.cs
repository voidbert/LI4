using System.Net.Mail;

using LI4.Dados;

namespace LI4.Negocio.Utilizadores;

public class UtilizadoresService
{
    public Utilizador IniciarSessao(string enderecoEletronico, string palavraPasse)
    {
        UtilizadorModel? utilizadorModel = UtilizadorRepository.Instancia.Obter(enderecoEletronico);
        if (utilizadorModel == null)
        {
            throw new UtilizadorNaoEncontradoException();
        }

        Utilizador utilizador = Utilizador.DeModel(utilizadorModel);

        if (!utilizador.PalavraPasseCorreta(palavraPasse))
        {
            throw new PalavraPasseIncorretaException();
        }

        if (!utilizador.PossivelIniciarSessao)
        {
            throw new ImpedidoDeIniciarSessaoException();
        }

        return utilizador;
    }

    public void RegistarUtilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, Utilizador.Tipo tipoDeConta)
    {
        try
        {
            new MailAddress(enderecoEletronico);
        }
        catch (Exception)
        {
            throw new EnderecoEletronicoInvalidoException();
        }

        BaseDeDados.Instancia.IniciarTransacao();

        if (UtilizadorRepository.Instancia.Obter(enderecoEletronico) != null)
        {
            throw new UtilizadorExistenteException();
        }

        Utilizador novoUtilizador = Utilizador.Criar(enderecoEletronico, nomeCivil, palavraPasse, true, tipoDeConta);
        UtilizadorModel novoUtilizadorModel = novoUtilizador.ParaModel();
        UtilizadorRepository.Instancia.Adicionar(novoUtilizadorModel);
        BaseDeDados.Instancia.CommitTransacao();
    }

    public List<Utilizador> ObterTodos()
    {
        return UtilizadorRepository.Instancia.ObterTodos().Select(model => Utilizador.DeModel(model)).ToList();
    }

    public void RegistarComoImpedidoDeIniciarSessao(string enderecoEletronico)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        UtilizadorModel? model = UtilizadorRepository.Instancia.Obter(enderecoEletronico);
        if (model == null)
        {
            throw new UtilizadorNaoEncontradoException();
        }

        model.PossivelIniciarSessao = false;
        UtilizadorRepository.Instancia.Atualizar(model);
        BaseDeDados.Instancia.CommitTransacao();
    }
}
