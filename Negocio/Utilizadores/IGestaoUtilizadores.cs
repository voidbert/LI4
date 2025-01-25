using LI4.Dados;

namespace LI4.Negocio.Utilizadores;

public interface IGestaoUtilizadores
{
    public List<Utilizador> ObterTodosOsUtilizadores();
    public Utilizador IniciarSessao(string enderecoEletronico, string palavraPasse);
    public void RegistarUtilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, Utilizador.Tipo tipoDeConta);
    public void RegistarComoImpedidoDeIniciarSessao(string enderecoEletronico);
}
