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

namespace LI4.Negocio.Producao;

[Serializable]
public class CarrinhoVazioException : Exception
{
    public CarrinhoVazioException() { }
    public CarrinhoVazioException(string message) : base(message) { }
    public CarrinhoVazioException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class EncomendaNaoEncontradaException : Exception
{
    public EncomendaNaoEncontradaException() { }
    public EncomendaNaoEncontradaException(string message) : base(message) { }
    public EncomendaNaoEncontradaException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class EstadoInvalidoException : Exception
{
    public EstadoInvalidoException() { }
    public EstadoInvalidoException(string message) : base(message) { }
    public EstadoInvalidoException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class OrdemProducaoVaziaException : Exception
{
    public OrdemProducaoVaziaException() { }
    public OrdemProducaoVaziaException(string message) : base(message) { }
    public OrdemProducaoVaziaException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class SemPartesException : Exception
{
    public SemPartesException() { }
    public SemPartesException(string message) : base(message) { }
    public SemPartesException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class OrdemProducaoInexistenteException : Exception
{
    public OrdemProducaoInexistenteException() { }
    public OrdemProducaoInexistenteException(string message) : base(message) { }
    public OrdemProducaoInexistenteException(string message, Exception innerException) : base(message, innerException) { }
}
