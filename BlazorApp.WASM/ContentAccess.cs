using BlazorApp;

public class ContentAccess : IContentAccess
{
    public ContentAccess(HttpClient httpClient) { 
        this.httpClient = httpClient;
    }
    HttpClient httpClient;
    public async Task<string> ReadJson(string path)
    {
        var text = await httpClient.GetStringAsync(path);
        return text;
    }
}
