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

public record EncomendaEVAsModel
{
    public int? Identificador { get; init; }
    public required string Cliente { get; set; }
    public required string Morada { get; set; }
    public required double Preco { get; set; }
    public required DateTime InstanteColocacao { get; set; }
    public DateTime? InstanteConfirmacao { get; set; }
    public DateTime? InstanteEntrega { get; set; }
    public DateTime? InstanteCancelamento { get; set; }
    public DateTime? InstanteDevolucao { get; set; }
    public required bool Aprovada { get; set; }
    public required Dictionary<int, int> Conteudo { get; set; }
}
