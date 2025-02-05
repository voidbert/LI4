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

public class Parte
{
    private Parte(int identificador, string nome, double preco, int quantidadeArmazem)
    {
        this.Identificador = identificador;
        this.Nome = nome;
        this.Preco = preco;
        this.QuantidadeArmazem = quantidadeArmazem;
    }

    public static Parte DeModel(ParteModel model)
    {
        return new Parte(model.Identificador, model.Nome, model.Preco, model.QuantidadeArmazem);
    }

    public ParteModel ParaModel()
    {
        return new ParteModel
        {
            Identificador = this.Identificador,
            Nome = this.Nome,
            Preco = this.Preco,
            QuantidadeArmazem = this.QuantidadeArmazem
        };
    }

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public int Identificador { get; init; }
    public string Nome { get; set; }
    public double Preco { get; set; }
    public int QuantidadeArmazem { get; set; }
}
