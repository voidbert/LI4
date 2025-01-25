namespace LI4.Dados;

public interface IEncomendaPartesRepository
{
    public List<EncomendaPartesModel> ObterTodas();
    public EncomendaPartesModel Adicionar(EncomendaPartesModel model);
}
