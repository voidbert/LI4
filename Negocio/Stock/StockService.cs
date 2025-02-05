/*
 * Copyright 2025 Ana Cerqueira, Humberto Gomes, João Torres, José Lopes, José Matos
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using LI4.Dados;

namespace LI4.Negocio.Stock;

public class StockService : IGestaoStock
{
    public StockService()
    {
        this.BaseDeDados = LI4.Dados.BaseDeDados.Instancia;
        this.Partes = ParteRepository.Instancia;
        this.Encomendas = EncomendaPartesRepository.Instancia;
    }

    public List<Parte> ObterTodasAsPartes()
    {
        return this.Partes.ObterTodas().Select(model => Parte.DeModel(model)).ToList();
    }

    public List<EncomendaPartes> ObterTodasAsEncomendasPartes()
    {
        return this.Encomendas.ObterTodas().Select(model => EncomendaPartes.DeModel(model)).ToList();
    }

    public void ColocarEncomendaPartes(EncomendaPartes encomenda)
    {
        if (encomenda.ConteudoRaw.Count == 0)
        {
            throw new EncomendaVaziaException();
        }
        encomenda.InstanteRealizacao = DateTime.Now;

        this.BaseDeDados.IniciarTransacao();

        foreach (KeyValuePair<Parte, int> entrada in encomenda.Conteudo)
        {
            ParteModel parte = entrada.Key.ParaModel();
            ParteModel novaParte = parte with { QuantidadeArmazem = parte.QuantidadeArmazem + entrada.Value };
            this.Partes.Atualizar(novaParte);
        }

        EncomendaPartesModel encomendaPartesModel = encomenda.ParaModel();
        Encomendas.Adicionar(encomendaPartesModel);
        this.BaseDeDados.CommitTransacao();
    }

    private IBaseDeDados BaseDeDados;
    private IParteRepository Partes;
    private IEncomendaPartesRepository Encomendas;
}
