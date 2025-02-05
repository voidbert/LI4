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

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

using LI4.Negocio;
using LI4.Negocio.Utilizadores;

namespace LI4.Apresentacao;

public class SessaoController
{
    private ICamadaNegocio CamadaNegocio;
    private ProtectedLocalStorage ProtectedLocalStorage;
    private NavigationManager NavigationManager;

    public SessaoController(ICamadaNegocio CamadaNegocio, ProtectedLocalStorage ProtectedLocalStorage, NavigationManager NavigationManager)
    {
        this.CamadaNegocio = CamadaNegocio;
        this.ProtectedLocalStorage = ProtectedLocalStorage;
        this.NavigationManager = NavigationManager;
    }

    public async Task<Utilizador> IniciarSessao(string enderecoEletronico, string palavraPasse)
    {
        Utilizador utilizador = CamadaNegocio.IniciarSessao(enderecoEletronico, palavraPasse);

        await ProtectedLocalStorage.SetAsync("enderecoEletronico", enderecoEletronico);
        await ProtectedLocalStorage.SetAsync("palavraPasse", palavraPasse);

        return await Task.FromResult(utilizador);
    }

    public async Task TerminarSessao()
    {
        await ProtectedLocalStorage.DeleteAsync("enderecoEletronico");
        await ProtectedLocalStorage.DeleteAsync("palavraPasse");
    }

    public async Task<Utilizador?> ObterUtilizadorComSessaoIniciada()
    {
        string? enderecoEletronico = (await ProtectedLocalStorage.GetAsync<string>("enderecoEletronico")).Value;
        string? palavraPasse = (await ProtectedLocalStorage.GetAsync<string>("palavraPasse")).Value;

        if (enderecoEletronico == null || palavraPasse == null)
        {
            return await Task.FromResult<Utilizador?>(null);
        }
        else
        {
            try
            {
                return await this.IniciarSessao(enderecoEletronico, palavraPasse);
            }
            catch (Exception)
            {
                // Credenciais mudaram / utilizador apagado
                await this.TerminarSessao();
                return await Task.FromResult<Utilizador?>(null);
            }
        }
    }

    public async Task RedirecionarConformeTipo(Utilizador.Tipo? desejado)
    {
        Utilizador? utilizadorAtual = await this.ObterUtilizadorComSessaoIniciada();

        Utilizador.Tipo? tipoAtual = null;
        if (utilizadorAtual != null)
        {
            tipoAtual = utilizadorAtual.TipoDeConta;
        }

        if (desejado != tipoAtual)
        {
            if (desejado == null)
            {
                string pagina = "/" + tipoAtual.ToString();
                NavigationManager.NavigateTo(pagina);
                return;
            }

            NavigationManager.NavigateTo("/");
            return;
        }
    }
}
