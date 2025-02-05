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

public class EVARepository : IEVARepository
{
    private static EVARepository? _Instancia;

    private EVARepository() { }

    public static EVARepository Instancia
    {
        get
        {
            if (EVARepository._Instancia == null)
                EVARepository._Instancia = new EVARepository();
            return EVARepository._Instancia;
        }
    }

    public EVAModel? Obter(int identificador)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM EVA WHERE Identificador=@identificador";
        List<EVAModel> lista = BaseDeDados.Instancia.LerDados<EVAModel, dynamic>(sql, new
        {
            identificador = identificador
        });

        if (lista.Count == 0)
        {
            return null;
        }

        string conteudoSql = "SELECT Parte AS [Key], Quantidade AS Value FROM EVAPartes WHERE EVA = @identificador";
        foreach (EVAModel model in lista)
        {
            List<KeyValuePair<int, int>> tuplos = BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(conteudoSql, new
            {
                identificador = identificador
            });

            lista[0].Partes = new Dictionary<int, int>(tuplos);
        }

        BaseDeDados.Instancia.CommitTransacao();
        return lista[0];
    }

    public List<EVAModel> ObterTodas()
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "SELECT * FROM EVA";
        List<EVAModel> lista = BaseDeDados.Instancia.LerDados<EVAModel, dynamic>(sql, new { });

        string conteudoSql = "SELECT Parte AS [Key], Quantidade AS Value FROM EVAPartes WHERE EVA = @eva";
        foreach (EVAModel model in lista)
        {
            List<KeyValuePair<int, int>> tuplos = BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(conteudoSql, new
            {
                eva = model.Identificador
            });

            model.Partes = new Dictionary<int, int>(tuplos);
        }

        BaseDeDados.Instancia.CommitTransacao();
        return lista;
    }

    public void Atualizar(EVAModel model)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string sql = "UPDATE EVA SET Nome=@nome, Imagem=@imagem, Preco=@preco, QuantidadeArmazem=@quantidadeArmazem WHERE Identificador=@identificador";

        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            identificador = model.Identificador,
            nome = model.Nome,
            imagem = model.Imagem,
            preco = model.Preco,
            quantidadeArmazem = model.QuantidadeArmazem
        });

        string apagarConteudoSql = "DELETE FROM EVAPartes WHERE EVA=@identificador";
        BaseDeDados.Instancia.EscreverDados<dynamic>(apagarConteudoSql, new
        {
            identificador = model.Identificador
        });

        string novoConteudoSql = "INSERT INTO EVAPartes VALUES (@eva, @parte, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in model.Partes)
        {
            BaseDeDados.Instancia.EscreverDados<dynamic>(novoConteudoSql, new
            {
                eva = model.Identificador,
                parte = entrada.Key,
                quantidade = entrada.Value
            });
        }

        BaseDeDados.Instancia.CommitTransacao();
    }
}
