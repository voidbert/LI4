namespace LI4.Dados;

public interface IParteRepository
{
    public ParteModel? Obter(int identificador);
    public List<ParteModel> ObterTodas();
    public void Atualizar(ParteModel model);
}
