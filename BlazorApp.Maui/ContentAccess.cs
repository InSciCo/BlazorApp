using Microsoft.Maui.Storage;
using System.IO;
using System.Threading.Tasks;

namespace BlazorApp.Maui;
public class ContentAccess : IContentAccess
{
    public async Task<string> ReadJson(string path)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync($"wwwroot/{path}");
        using var streamReader = new StreamReader(stream);
        var text = await streamReader.ReadToEndAsync();
        return text;
    }
}
