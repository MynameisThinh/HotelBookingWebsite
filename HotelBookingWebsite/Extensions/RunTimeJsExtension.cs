using Microsoft.JSInterop;

namespace HotelBookingWebsite.Extensions
{
    public static class RunTimeJsExtension
    {
        public static async Task AlertAsync(this IJSRuntime jSRuntime,string message) =>
            await jSRuntime.InvokeVoidAsync("window.alert", message);
        public static async Task<bool> ConfirnAsync(this IJSRuntime jSRuntime, string message) => 
            await jSRuntime.InvokeAsync<bool>("window.confirm", message);

        public static async Task<string?> PromptAsync(this IJSRuntime jSRuntime, string message) =>
            await jSRuntime.InvokeAsync<string?>("window.prompt", message);
    }
}
