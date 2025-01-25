namespace LI4.Dados;

public interface IUtilizadorRepository
{
    public UtilizadorModel? Obter(string enderecoEletronico);
    public List<UtilizadorModel> ObterTodos();
    public void Adicionar(UtilizadorModel model);
    public void Atualizar(UtilizadorModel model);
}
