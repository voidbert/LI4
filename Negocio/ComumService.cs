using LI4.Dados;

namespace LI4.Negocio;

public class ComumService
{
    public EVA? ObterEVA(int identificador)
    {
        EVAModel? model = EVARepository.Instancia.Obter(identificador);
        if (model == null)
        {
            return null;
        }

        return EVA.DeModel(model);
    }

    public List<EVA> ObterTodasAsEVAs()
    {
        return EVARepository.Instancia.ObterTodas().Select(model => EVA.DeModel(model)).ToList();
    }
}
