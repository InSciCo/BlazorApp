using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp;

public interface IContentAccess
{
    /// <summary>
    /// Read json file from _content folder.
    /// WASM project implements this using
    ///     Use HttpClient
    /// MAUI project implements this using
    ///     FileSystem.OpenAppPackageFileAsync(String)
    /// if in MAUI. 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Task<string> ReadJson(string path);

}
