using LI4.Apresentacao;
using LI4.Negocio;

namespace LI4;

public class Program
{
    public static void Main(String[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddSingleton<ICamadaNegocio, CamadaNegocioService>();

        WebApplication app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        app.Run();
    }
}
