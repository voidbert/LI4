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

using LI4.Negocio.Producao;
using LI4.Negocio.Stock;
using LI4.Negocio.Utilizadores;

namespace LI4.Negocio;

public interface ICamadaNegocio
{
    public List<EVA> ObterTodasAsEVAs();

    public CarrinhoCompras ObterCarrinho(string cliente);
    public List<EncomendaEVAs> ObterTodasAsEncomendasEVAs();
    public void AtualizarCarrinho(CarrinhoCompras carrinho);
    public void ColocarEncomendaEVAs(EncomendaEVAs encomenda);
    public void RejeitarEncomendaEVAs(int identificador);
    public void AprovarEncomendaEVAs(int identificador);
    public void CancelarEncomendaEVAs(int identificador);
    public void DevolverEncomendaEVAs(int identificador);
    public void ColocarOrdemProducao(OrdemProducao ordemProducao);
    public void RegistarOrdemProducaoComoVisualizada(int identificador);

    public List<Parte> ObterTodasAsPartes();
    public List<EncomendaPartes> ObterTodasAsEncomendasPartes();
    public void ColocarEncomendaPartes(EncomendaPartes encomenda);

    public List<Utilizador> ObterTodosOsUtilizadores();
    public Utilizador IniciarSessao(string enderecoEletronico, string palavraPasse);
    public void RegistarUtilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, Utilizador.Tipo tipoDeConta);
    public void RegistarComoImpedidoDeIniciarSessao(string enderecoEletronico);
}
