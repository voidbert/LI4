using LI4.Components;

public class Program {
    public static void Main(String[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        WebApplication app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        app.Run();
    }
}
