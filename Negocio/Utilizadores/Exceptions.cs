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

[Serializable]
public class TipoDeContaInexistenteException : Exception
{
    public TipoDeContaInexistenteException() { }
    public TipoDeContaInexistenteException(string message) : base(message) { }
    public TipoDeContaInexistenteException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class UtilizadorNaoEncontradoException : Exception
{
    public UtilizadorNaoEncontradoException() { }
    public UtilizadorNaoEncontradoException(string message) : base(message) { }
    public UtilizadorNaoEncontradoException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class PalavraPasseIncorretaException : Exception
{
    public PalavraPasseIncorretaException() { }
    public PalavraPasseIncorretaException(string message) : base(message) { }
    public PalavraPasseIncorretaException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class ImpedidoDeIniciarSessaoException : Exception
{
    public ImpedidoDeIniciarSessaoException() { }
    public ImpedidoDeIniciarSessaoException(string message) : base(message) { }
    public ImpedidoDeIniciarSessaoException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class EnderecoEletronicoInvalidoException : Exception
{
    public EnderecoEletronicoInvalidoException() { }
    public EnderecoEletronicoInvalidoException(string message) : base(message) { }
    public EnderecoEletronicoInvalidoException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class UtilizadorExistenteException : Exception
{
    public UtilizadorExistenteException() { }
    public UtilizadorExistenteException(string message) : base(message) { }
    public UtilizadorExistenteException(string message, Exception innerException) : base(message, innerException) { }
}
