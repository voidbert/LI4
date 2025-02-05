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

namespace LI4.Dados;

public class EncomendaPartesRepository : IEncomendaPartesRepository
{
    private static EncomendaPartesRepository? _Instancia;

    private EncomendaPartesRepository() { }

    public static EncomendaPartesRepository Instancia
    {
        get
        {
            if (EncomendaPartesRepository._Instancia == null)
                EncomendaPartesRepository._Instancia = new EncomendaPartesRepository();
            return EncomendaPartesRepository._Instancia;
        }
    }

    public List<EncomendaPartesModel> ObterTodas()
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM EncomendaPartes";
        List<EncomendaPartesModel> lista = BaseDeDados.Instancia.LerDados<EncomendaPartesModel, dynamic>(sql, new { });

        string conteudoSql = "SELECT Parte AS [Key], Quantidade AS Value FROM ConteudoEncomendaPartes WHERE Encomenda = @encomenda";
        foreach (EncomendaPartesModel model in lista)
        {
            List<KeyValuePair<int, int>> tuplos = BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(conteudoSql, new
            {
                encomenda = model.Identificador
            });

            model.Conteudo = new Dictionary<int, int>(tuplos);
        }

        BaseDeDados.Instancia.CommitTransacao();
        return lista;
    }

    public EncomendaPartesModel Adicionar(EncomendaPartesModel model)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "INSERT INTO EncomendaPartes(InstanteRealizacao, Preco, Funcionario) VALUES (@instanteRealizacao, @preco, @funcionario)";
        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            instanteRealizacao = model.InstanteRealizacao,
            preco = model.Preco,
            funcionario = model.Funcionario,
        });

        string idSql = "SELECT MAX(Identificador) FROM EncomendaPartes";
        int identificador = BaseDeDados.Instancia.LerDados<int, dynamic>(idSql, new { })[0];

        string conteudosSql = "INSERT INTO ConteudoEncomendaPartes (Encomenda, Parte, Quantidade) VALUES (@encomenda, @parte, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in model.Conteudo)
        {
            if (entrada.Value > 0)
            {
                BaseDeDados.Instancia.EscreverDados<dynamic>(conteudosSql, new
                {
                    encomenda = identificador,
                    parte = entrada.Key,
                    quantidade = entrada.Value
                });
            }
        }

        BaseDeDados.Instancia.CommitTransacao();
        return model with { Identificador = identificador };
    }
}
