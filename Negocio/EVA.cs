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
using LI4.Negocio.Stock;

namespace LI4.Negocio;

public class EVA
{
    private EVA(int identificador, string nome, string imagem, double preco, int quantidadeArmazem, Dictionary<int, int> partes)
    {
        this.Identificador = identificador;
        this.Nome = nome;
        this.Imagem = imagem;
        this.Preco = preco;
        this.QuantidadeArmazem = quantidadeArmazem;
        this._PartesRaw = partes.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
    }

    public static EVA DeModel(EVAModel model)
    {
        return new EVA(model.Identificador, model.Nome, model.Imagem, model.Preco, model.QuantidadeArmazem, model.Partes);
    }

    public EVAModel ParaModel()
    {
        return new EVAModel
        {
            Identificador = this.Identificador,
            Nome = this.Nome,
            Imagem = this.Imagem,
            Preco = this.Preco,
            QuantidadeArmazem = this.QuantidadeArmazem,
            Partes = this.PartesRaw
        };
    }

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public int Identificador { get; init; }
    public string Nome { get; set; }
    public string Imagem { get; set; }
    public double Preco { get; set; }
    public int QuantidadeArmazem { get; set; }
    private Dictionary<int, int> _PartesRaw;

    public Dictionary<int, int> PartesRaw
    {
        get
        {
            return this._PartesRaw.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
        }
        set
        {
            this._PartesRaw = value.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
        }
    }

    public Dictionary<Parte, int> Partes
    {
        get
        {
            return this._PartesRaw.ToDictionary(entrada => Parte.DeModel(ParteRepository.Instancia.Obter(entrada.Key)!), entrada => entrada.Value);
        }
        set
        {
            this._PartesRaw = value.ToDictionary(entrada => entrada.Key.Identificador, entrada => entrada.Value);
        }
    }
}
