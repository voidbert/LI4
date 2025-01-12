using Microsoft.JSInterop;

namespace LI4.Apresentacao;

public class Alert
{
    private IJSRuntime JSRuntime { get; set; }

    public Alert(IJSRuntime JSRuntime)
    {
        this.JSRuntime = JSRuntime;
    }

    public async void Launch(string message)
    {
        await JSRuntime!.InvokeVoidAsync("alert", message);
    }
}
