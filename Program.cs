using LI4.Apresentacao;
using LI4.Negocio;
using LI4.Negocio.Stock;
using LI4.Negocio.Utilizadores;

namespace LI4;

public class Program
{
    public static void Main(String[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddSingleton<ComumService>();
        builder.Services.AddSingleton<UtilizadoresService>();
        builder.Services.AddSingleton<StockService>();

        WebApplication app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        app.Run();
    }
}
