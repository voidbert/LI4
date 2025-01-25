namespace LI4.Dados;

public interface IEVARepository
{
    public EVAModel? Obter(int identificador);
    public List<EVAModel> ObterTodas();
    public void Atualizar(EVAModel model);
}
