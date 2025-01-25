namespace LI4.Dados;

public interface IEncomendaEVAsRepository
{
    public EncomendaEVAsModel? Obter(int identificador);
    public List<EncomendaEVAsModel> ObterTodas();
    public EncomendaEVAsModel Adicionar(EncomendaEVAsModel model);
    public void Atualizar(EncomendaEVAsModel model);
    public void Eliminar(int identificador);
}
