using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

using LI4.Negocio.Utilizadores;

namespace LI4.Apresentacao;

public class SessaoController
{
    private UtilizadoresService UtilizadoresService;
    private ProtectedLocalStorage ProtectedLocalStorage;
    private NavigationManager NavigationManager;

    public SessaoController(UtilizadoresService UtilizadoresService, ProtectedLocalStorage ProtectedLocalStorage, NavigationManager NavigationManager)
    {
        this.UtilizadoresService = UtilizadoresService;
        this.ProtectedLocalStorage = ProtectedLocalStorage;
        this.NavigationManager = NavigationManager;
    }

    public async Task<Utilizador> IniciarSessao(string enderecoEletronico, string palavraPasse)
    {
        Utilizador utilizador = UtilizadoresService.IniciarSessao(enderecoEletronico, palavraPasse);

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
