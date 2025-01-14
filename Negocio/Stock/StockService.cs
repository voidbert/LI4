using System.Net.Mail;

using LI4.Dados;

namespace LI4.Negocio.Stock;

public class StockService
{
    public async Task<Parte?> ObterParte(int identificador)
    {
        ParteModel? model = await PartesRepository.Instancia.Obter(identificador);
        if (model == null)
        {
            return null;
        }

        return Parte.DeModel(model);
    }

    public async Task<List<Parte>> ObterTodasAsPartes()
    {
        List<ParteModel> modelos = await PartesRepository.Instancia.ObterTodas();
        return modelos.Select(model => Parte.DeModel(model)).ToList();
    }

    public async Task ColocarEncomenda(EncomendaPartes encomenda)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        int partes = 0;
        foreach (KeyValuePair<int, int> entrada in encomenda.Conteudo)
        {
            if (entrada.Value != 0)
            {
                ParteModel? parte = await PartesRepository.Instancia.Obter(entrada.Key);
                if (parte == null)
                {
                    BaseDeDados.Instancia.AbortarTransacao();
                    throw new ParteNaoEncontradaException();
                }

                ParteModel novaParte = parte with { QuantidadeArmazem = parte.QuantidadeArmazem + entrada.Value };
                await PartesRepository.Instancia.Atualizar(novaParte);

                partes++;
            }
        }

        if (partes == 0)
        {
            BaseDeDados.Instancia.AbortarTransacao();
            throw new EncomendaVaziaException();
        }

        EncomendaPartesModel encomendaPartesModel = new EncomendaPartesModel
        {
            Identificador = encomenda.Identificador,
            InstanteRealizacao = encomenda.InstanteRealizacao,
            Preco = encomenda.Preco,
            Funcionario = (await encomenda.Funcionario).EnderecoEletronico,
            Conteudo = encomenda.Conteudo
        };

        await EncomendaPartesRepository.Instancia.Adicionar(encomendaPartesModel);
        BaseDeDados.Instancia.CommitTransacao();
    }
}
