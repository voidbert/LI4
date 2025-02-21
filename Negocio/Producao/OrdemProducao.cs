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
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio.Producao;

public class OrdemProducao
{
    public OrdemProducao(DateTime instanteEmissao, Utilizador funcionario)
    {
        this.Identificador = null;
        this.FuncionarioRaw = funcionario.EnderecoEletronico;
        this.InstanteEmissao = instanteEmissao;
        this.Visualizada = false;
        this._ConteudoRaw = new Dictionary<int, int>();
    }

    private OrdemProducao(int? identificador, string funcionario, DateTime instanteEmissao, bool visualizada, Dictionary<int, int> conteudo)
    {
        this.Identificador = identificador;
        this.FuncionarioRaw = funcionario;
        this.InstanteEmissao = instanteEmissao;
        this.Visualizada = visualizada;
        this._ConteudoRaw = conteudo.ToDictionary(entrada => entrada.Key, entrada => entrada.Value);
    }

    public static OrdemProducao DeModel(OrdemProducaoModel model)
    {
        return new OrdemProducao(model.Identificador, model.Funcionario, model.InstanteEmissao, model.Visualizada, model.Conteudo);
    }

    public OrdemProducaoModel ParaModel()
    {
        return new OrdemProducaoModel
        {
            Identificador = this.Identificador,
            Funcionario = this.FuncionarioRaw,
            InstanteEmissao = this.InstanteEmissao,
            Visualizada = this.Visualizada,
            Conteudo = this.ConteudoRaw
        };
    }

    public override int GetHashCode()
    {
        return this.Identificador.GetHashCode();
    }

    public void DefinirQuantidadeDeEVA(EVA eva, int quantidade)
    {
        if (this._ConteudoRaw.ContainsKey(eva.Identificador) && quantidade == 0)
        {
            this._ConteudoRaw.Remove(eva.Identificador);
        }
        else
        {
            this._ConteudoRaw[eva.Identificador] = quantidade;
        }
    }

    public int? Identificador { get; init; }
    public string FuncionarioRaw { get; set; }
    public DateTime InstanteEmissao { get; set; }
    public bool Visualizada { get; set; }
    private Dictionary<int, int> _ConteudoRaw { get; set; }

    public GestorDeProducao Funcionario
    {
        get
        {
            return (GestorDeProducao)Utilizador.DeModel(UtilizadorRepository.Instancia.Obter(this.FuncionarioRaw)!);
        }
        set
        {
            this.FuncionarioRaw = value.EnderecoEletronico;
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
