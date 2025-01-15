using LI4.Dados;

namespace LI4.Negocio.Producao;

public class ProducaoService
{
    public async Task ColocarOrdemProducao(OrdemProducao ordemProducao)
    {
        if (ordemProducao.Conteudo.Count == 0)
            throw new OrdemProducaoVaziaException();

        BaseDeDados.Instancia.IniciarTransacao();

        foreach (KeyValuePair<int, int> entrada in ordemProducao.Conteudo)
        {
            EVAModel? eva = await EVARepository.Instancia.Obter(entrada.Key);
            if (eva == null)
            {
                BaseDeDados.Instancia.AbortarTransacao();
                throw new EVANaoEncontradaException();
            }

            foreach (KeyValuePair<int, int> parteEntrada in eva.Partes)
            {
                ParteModel? parte = await PartesRepository.Instancia.Obter(parteEntrada.Key);
                if (parte == null || parte.QuantidadeArmazem < parteEntrada.Value * entrada.Value)
                {
                    BaseDeDados.Instancia.AbortarTransacao();
                    throw new SemPartesException();
                }

                ParteModel novaParte = parte with { QuantidadeArmazem = parte.QuantidadeArmazem - parteEntrada.Value * entrada.Value };
                await PartesRepository.Instancia.Atualizar(novaParte);
            }

            EVAModel novaEVA = eva with { QuantidadeArmazem = eva.QuantidadeArmazem + entrada.Value };
            await EVARepository.Instancia.Atualizar(novaEVA);
        }

        OrdemProducaoModel ordemProducaoModel = new OrdemProducaoModel
        {
            Identificador = ordemProducao.Identificador,
            Funcionario = (await ordemProducao.Funcionario).EnderecoEletronico,
            InstanteEmissao = ordemProducao.InstanteEmissao,
            Visualizada = ordemProducao.Visualizada,
            Conteudo = ordemProducao.Conteudo
        };

        await OrdemProducaoRepository.Instancia.Adicionar(ordemProducaoModel);
        BaseDeDados.Instancia.CommitTransacao();

        // TODO - suprir encomendas
    }
}
