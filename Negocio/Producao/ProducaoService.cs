using LI4.Dados;
using LI4.Negocio.Encomendas;
using LI4.Negocio.Stock;

namespace LI4.Negocio.Producao;

public class ProducaoService
{
    public void ColocarOrdemProducao(OrdemProducao ordemProducao)
    {
        if (ordemProducao.Conteudo.Count == 0)
            throw new OrdemProducaoVaziaException();

        BaseDeDados.Instancia.IniciarTransacao();

        // Remover partes necessárias e colocar novas EVAs no armazém 
        foreach (KeyValuePair<EVA, int> evaEntrada in ordemProducao.Conteudo)
        {
            EVAModel eva = evaEntrada.Key.ParaModel();

            foreach (KeyValuePair<Parte, int> parteEntrada in evaEntrada.Key.Partes)
            {
                ParteModel parte = parteEntrada.Key.ParaModel();

                if (parteEntrada.Key.QuantidadeArmazem < parteEntrada.Value * evaEntrada.Value)
                {
                    BaseDeDados.Instancia.AbortarTransacao();
                    throw new SemPartesException();
                }

                ParteModel novaParte = parte with { QuantidadeArmazem = parte.QuantidadeArmazem - parteEntrada.Value * evaEntrada.Value };
                ParteRepository.Instancia.Atualizar(novaParte);
            }

            EVAModel novaEVA = eva with { QuantidadeArmazem = eva.QuantidadeArmazem + evaEntrada.Value };
            EVARepository.Instancia.Atualizar(novaEVA);
        }

        // Registar ordem de producao
        OrdemProducaoRepository.Instancia.Adicionar(ordemProducao.ParaModel());

        (new EncomendasService()).TentarSatisfazerTodasAsEncomendas();
        BaseDeDados.Instancia.CommitTransacao();
    }

    public void RegistarOrdemProducaoComoVisualizada(int identificador)
    {
        BaseDeDados.Instancia.IniciarTransacao();
        OrdemProducaoModel? model = OrdemProducaoRepository.Instancia.Obter(identificador);

        if (model == null)
        {
            BaseDeDados.Instancia.AbortarTransacao();
            throw new OrdemProducaoInexistenteException();
        }

        OrdemProducaoRepository.Instancia.Atualizar(model with { Visualizada = true });
        BaseDeDados.Instancia.CommitTransacao();
    }
}
