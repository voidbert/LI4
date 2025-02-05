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

public class Cliente : Utilizador
{
    public Cliente(string enderecoEletronico, string nomeCivil, byte[] palavraPasse, bool possivelIniciarSessao, List<int> encomendas)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao)
    {
        this._EncomendasRaw = encomendas.ToList();
    }

    public Cliente(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao, List<int> encomendas)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao)
    {
        this._EncomendasRaw = encomendas.ToList();
    }

    private List<int> _EncomendasRaw { get; set; }

    public override Tipo TipoDeConta
    {
        get
        {
            return Tipo.Cliente;
        }
    }

    public List<int> EncomendasRaw
    {
        get
        {
            return this._EncomendasRaw.ToList();
        }
        set
        {
            this._EncomendasRaw = value.ToList();
        }
    }

    public List<EncomendaEVAs> Encomendas
    {
        get
        {
            return this._EncomendasRaw.Select(e => EncomendaEVAs.DeModel(EncomendaEVAsRepository.Instancia.Obter(e)!)).ToList();
        }
        set
        {
            this._EncomendasRaw = value.Select(e => e.Identificador!.Value).ToList();
        }
    }
}
