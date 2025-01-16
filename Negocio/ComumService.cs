using LI4.Dados;

namespace LI4.Negocio;

public class ComumService
{
    public async Task<EVA?> ObterEVA(int identificador)
    {
        EVAModel? model = await EVARepository.Instancia.Obter(identificador);
        if (model == null)
        {
            return null;
        }

        return EVA.DeModel(model);
    }

    public async Task<List<EVA>> ObterTodasAsEVAs()
    {
        List<EVAModel> modelos = await EVARepository.Instancia.ObterTodas();
        return modelos.Select(model => EVA.DeModel(model)).ToList();
    }
}
