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

namespace LI4.Negocio.Utilizadores;

public class Administrador : Utilizador
{
    public Administrador(string enderecoEletronico, string nomeCivil, byte[] palavraPasse, bool possivelIniciarSessao)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao) { }

    public Administrador(string enderecoEletronico, string nomeCivil, string palavraPasse, bool possivelIniciarSessao)
        : base(enderecoEletronico, nomeCivil, palavraPasse, possivelIniciarSessao) { }

    public override Tipo TipoDeConta
    {
        get
        {
            return Tipo.Administrador;
        }
    }
}
