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
using LI4.Negocio.Producao;

namespace LI4.Negocio.Utilizadores;

public class GestorDeProducao : Utilizador
{
    public GestorDeProducao(string enderecoEletronico, string nomeCivil, byte[] palavraPasse, bool possivelIniciarSessao, List<int> ordensProducao)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao)
    {
        this._OrdensProducaoRaw = ordensProducao.ToList();
    }

    public GestorDeProducao(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao, List<int> ordensProducao)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao)
    {

        this._OrdensProducaoRaw = ordensProducao.ToList();
    }

    private List<int> _OrdensProducaoRaw { get; set; }

    public override Tipo TipoDeConta
    {
        get
        {
            return Tipo.GestorDeProducao;
        }
    }

    public List<int> OrdensProducaoRaw
    {
        get
        {
            return this._OrdensProducaoRaw.ToList();
        }
        set
        {
            this._OrdensProducaoRaw = value.ToList();
        }
    }

    public List<OrdemProducao> OrdensProducao
    {
        get
        {
            return this._OrdensProducaoRaw.Select(e => OrdemProducao.DeModel(OrdemProducaoRepository.Instancia.Obter(e)!)).ToList();
        }
        set
        {
            this._OrdensProducaoRaw = value.Select(e => e.Identificador!.Value).ToList();
        }
    }
}
