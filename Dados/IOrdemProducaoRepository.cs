namespace LI4.Dados;

public interface IOrdemProducaoRepository
{
    public OrdemProducaoModel? Obter(int identificador);
    public OrdemProducaoModel Adicionar(OrdemProducaoModel model);
    public void Atualizar(OrdemProducaoModel model);
}
