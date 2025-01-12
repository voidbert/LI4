using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

using LI4.Negocio.Utilizadores;

namespace LI4.Apresentacao;

public class SessaoController
{
    private UtilizadoresService UtilizadoresService;
    private ProtectedLocalStorage ProtectedLocalStorage;
    private NavigationManager NavigationManager;

    public SessaoController(
            UtilizadoresService UtilizadoresService,
            ProtectedLocalStorage ProtectedLocalStorage,
            NavigationManager NavigationManager)
    {

        this.UtilizadoresService = UtilizadoresService;
        this.ProtectedLocalStorage = ProtectedLocalStorage;
        this.NavigationManager = NavigationManager;
    }

    public async Task<Utilizador.Tipo> IniciarSessao(string enderecoEletronico, string palavraPasse)
    {
        Utilizador.Tipo tipo = await UtilizadoresService.IniciarSessao(enderecoEletronico, palavraPasse);

        await ProtectedLocalStorage.SetAsync("enderecoEletronico", enderecoEletronico);
        await ProtectedLocalStorage.SetAsync("palavraPasse", palavraPasse);

        return await Task.FromResult(tipo);
    }

    public async Task TerminarSessao()
    {
        await ProtectedLocalStorage.DeleteAsync("enderecoEletronico");
        await ProtectedLocalStorage.DeleteAsync("palavraPasse");
    }

    public async Task<Utilizador.Tipo?> GetTipoDeUtilizadorComSessaoIniciada()
    {
        string? enderecoEletronico = (await ProtectedLocalStorage.GetAsync<string>("enderecoEletronico")).Value;
        string? palavraPasse = (await ProtectedLocalStorage.GetAsync<string>("palavraPasse")).Value;

        if (enderecoEletronico == null || palavraPasse == null)
        {
            return await Task.FromResult<Utilizador.Tipo?>(null);
        }
        else
        {
            try
            {
                return await this.IniciarSessao(enderecoEletronico, palavraPasse);
            }
            catch (Exception)
            {
                await this.TerminarSessao();
                return await Task.FromResult<Utilizador.Tipo?>(null);
            }
        }
    }

    public async void RedirecionarConformeTipo(Utilizador.Tipo? desejado)
    {
        Utilizador.Tipo? atual = await this.GetTipoDeUtilizadorComSessaoIniciada();
        if (desejado != atual)
        {
            if (desejado == null)
            {
                string pagina = "/" + atual.ToString();
                NavigationManager.NavigateTo(pagina);
                return;
            }

            NavigationManager.NavigateTo("/");
            return;
        }
    }
}
