namespace LI4.Negocio.Stock;

public interface IGestaoStock
{
    public List<Parte> ObterTodasAsPartes();
    public List<EncomendaPartes> ObterTodasAsEncomendasPartes();
    public void ColocarEncomendaPartes(EncomendaPartes encomenda);
}
