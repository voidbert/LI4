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
using LI4.Negocio;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Producao;

public class CarrinhoCompras
{
    private CarrinhoCompras(string cliente, Dictionary<int, int> conteudo)
    {
        this.ClienteRaw = cliente;
        this._ConteudoRaw = conteudo.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
    }

    public static CarrinhoCompras DeModel(CarrinhoComprasModel model)
    {
        return new CarrinhoCompras(model.Cliente, model.Conteudo);
    }

    public CarrinhoComprasModel ParaModel()
    {
        return new CarrinhoComprasModel
        {
            Cliente = this.ClienteRaw,
            Conteudo = this.ConteudoRaw
        };
    }

    public string ClienteRaw { get; init; }
    private Dictionary<int, int> _ConteudoRaw;

    public void DefinirQuantidadeDeEVA(EVA eva, int quantidade)
    {
        if (quantidade == 0)
        {
            if (this._ConteudoRaw.ContainsKey(eva.Identificador))
            {
                this._ConteudoRaw.Remove(eva.Identificador);
            }
        }
        else
        {
            this._ConteudoRaw[eva.Identificador] = quantidade;
        }
    }

    public double CalcularPreco()
    {
        double preco = 0.0;

        foreach (KeyValuePair<EVA, int> entrada in this.Conteudo)
        {
            preco += entrada.Key.Preco * entrada.Value;
        }

        return preco;
    }

    public Cliente Cliente
    {
        get
        {
            return (Cliente)Utilizador.DeModel(UtilizadorRepository.Instancia.Obter(this.ClienteRaw)!);
        }
    }

    public Dictionary<int, int> ConteudoRaw
    {
        get
        {
            return this._ConteudoRaw.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
        }
        set
        {
            this._ConteudoRaw = value.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
        }
    }

    public Dictionary<EVA, int> Conteudo
    {
        get
        {
            return this._ConteudoRaw.ToDictionary(entrada => EVA.DeModel(EVARepository.Instancia.Obter(entrada.Key)!), entrada => entrada.Value);
        }
        set
        {
            this._ConteudoRaw = value.ToDictionary(entrada => entrada.Key.Identificador, entrada => entrada.Value);
        }
    }
}
