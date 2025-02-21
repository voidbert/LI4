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

using System.Net.Mail;

using LI4.Dados;

namespace LI4.Negocio.Utilizadores;

public class UtilizadoresService : IGestaoUtilizadores
{
    public UtilizadoresService()
    {
        this.BaseDeDados = LI4.Dados.BaseDeDados.Instancia;
        this.Utilizadores = UtilizadorRepository.Instancia;
        this.Encomendas = EncomendaEVAsRepository.Instancia;
    }

    public List<Utilizador> ObterTodosOsUtilizadores()
    {
        return this.Utilizadores.ObterTodos().Select(model => Utilizador.DeModel(model)).ToList();
    }

    public Utilizador IniciarSessao(string enderecoEletronico, string palavraPasse)
    {
        UtilizadorModel? utilizadorModel = this.Utilizadores.Obter(enderecoEletronico);
        if (utilizadorModel == null)
        {
            throw new UtilizadorNaoEncontradoException();
        }

        Utilizador utilizador = Utilizador.DeModel(utilizadorModel);

        if (!utilizador.PalavraPasseCorreta(palavraPasse))
        {
            throw new PalavraPasseIncorretaException();
        }

        if (!utilizador.PossivelIniciarSessao)
        {
            throw new ImpedidoDeIniciarSessaoException();
        }

        return utilizador;
    }

    public void RegistarUtilizador(string enderecoEletronico, string nomeCivil, string palavraPasse, Utilizador.Tipo tipoDeConta)
    {
        try
        {
            new MailAddress(enderecoEletronico);
        }
        catch (Exception)
        {
            throw new EnderecoEletronicoInvalidoException();
        }

        this.BaseDeDados.IniciarTransacao();

        if (this.Utilizadores.Obter(enderecoEletronico) != null)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new UtilizadorExistenteException();
        }

        Utilizador novoUtilizador = Utilizador.Criar(enderecoEletronico, nomeCivil, palavraPasse, true, tipoDeConta);
        UtilizadorModel novoUtilizadorModel = novoUtilizador.ParaModel();
        this.Utilizadores.Adicionar(novoUtilizadorModel);
        this.BaseDeDados.CommitTransacao();
    }

    public void RegistarComoImpedidoDeIniciarSessao(string enderecoEletronico)
    {
        this.BaseDeDados.IniciarTransacao();

        UtilizadorModel? model = this.Utilizadores.Obter(enderecoEletronico);
        if (model == null)
        {
            this.BaseDeDados.AbortarTransacao();
            throw new UtilizadorNaoEncontradoException();
        }

        model.PossivelIniciarSessao = false;
        this.Utilizadores.Atualizar(model);

        if (model.TipoDeConta == "C")
        {
            foreach (int encomendaIdentificador in model.Encomendas!)
            {
                EncomendaEVAsModel encomendaModel = this.Encomendas.Obter(encomendaIdentificador)!;
                if (encomendaModel.InstanteEntrega == null)
                {
                    this.Encomendas.Eliminar(encomendaIdentificador);
                }
            }
        }

        this.BaseDeDados.CommitTransacao();
    }

    private IBaseDeDados BaseDeDados;
    private IUtilizadorRepository Utilizadores;
    private IEncomendaEVAsRepository Encomendas;
}
