using Microsoft.JSInterop;

namespace Blazorise.FileSystemAccess
{
    // This class wraps the browser-fs-access project in a .NET class for easy consumption.
    // The associated JavaScript module is loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class FileSystemAccess : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public FileSystemAccess(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Blazorise.FileSystemAccess/vendor/index.modern.js").AsTask());
        }

        public async Task FileOpen()
        {

        }

        public async Task DirectoryOpen()
        {

        }
        public async Task FileSave()
        {

        }


        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}