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

public class ParteRepository : IParteRepository
{
    private static ParteRepository? _Instancia;

    private ParteRepository() { }

    public static ParteRepository Instancia
    {
        get
        {
            if (ParteRepository._Instancia == null)
                ParteRepository._Instancia = new ParteRepository();
            return ParteRepository._Instancia;
        }
    }

    public ParteModel? Obter(int identificador)
    {
        string sql = "SELECT * FROM Parte WHERE Identificador=@identificador";
        List<ParteModel> lista = BaseDeDados.Instancia.LerDados<ParteModel, dynamic>(sql, new
        {
            identificador = identificador
        });

        if (lista.Count == 0)
            return null;
        return lista[0];
    }

    public List<ParteModel> ObterTodas()
    {
        string sql = "SELECT * FROM Parte";
        List<ParteModel> lista = BaseDeDados.Instancia.LerDados<ParteModel, dynamic>(sql, new { });
        return lista;
    }

    public void Atualizar(ParteModel model)
    {
        string sql = "UPDATE Parte SET Nome=@nome, Preco=@preco, QuantidadeArmazem=@quantidadeArmazem WHERE Identificador=@identificador";

        BaseDeDados.Instancia.EscreverDados<dynamic>(sql, new
        {
            identificador = model.Identificador,
            nome = model.Nome,
            preco = model.Preco,
            quantidadeArmazem = model.QuantidadeArmazem
        });
    }
}
