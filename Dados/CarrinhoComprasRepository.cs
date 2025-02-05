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

public class CarrinhoComprasRepository : ICarrinhoComprasRepository
{
    private static CarrinhoComprasRepository? _Instancia;

    private CarrinhoComprasRepository() { }

    public static CarrinhoComprasRepository Instancia
    {
        get
        {
            if (CarrinhoComprasRepository._Instancia == null)
                CarrinhoComprasRepository._Instancia = new CarrinhoComprasRepository();
            return CarrinhoComprasRepository._Instancia;
        }
    }

    public CarrinhoComprasModel Obter(string cliente)
    {
        string sql = "SELECT EVA AS [Key], Quantidade AS Value FROM CarrinhoCompras WHERE Cliente=@cliente";
        List<KeyValuePair<int, int>> lista = BaseDeDados.Instancia.LerDados<KeyValuePair<int, int>, dynamic>(sql, new
        {
            cliente = cliente
        });

        return new CarrinhoComprasModel
        {
            Cliente = cliente,
            Conteudo = new Dictionary<int, int>(lista)
        };
    }

    public void Atualizar(CarrinhoComprasModel carrinhoCompras)
    {
        BaseDeDados.Instancia.IniciarTransacao();

        string apagarSql = "DELETE FROM CarrinhoCompras WHERE Cliente=@cliente";
        BaseDeDados.Instancia.EscreverDados<dynamic>(apagarSql, new
        {
            cliente = carrinhoCompras.Cliente
        });

        string inserirSql = "INSERT INTO CarrinhoCompras (Cliente, EVA, Quantidade) VALUES (@cliente, @eva, @quantidade)";
        foreach (KeyValuePair<int, int> entrada in carrinhoCompras.Conteudo)
        {
            BaseDeDados.Instancia.EscreverDados<dynamic>(inserirSql, new
            {
                cliente = carrinhoCompras.Cliente,
                eva = entrada.Key,
                quantidade = entrada.Value
            });
        }

        BaseDeDados.Instancia.CommitTransacao();
    }
}
