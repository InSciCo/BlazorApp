using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.ViewModels;

public static class ConfigureBlazorAppViewModels
{
    public static IServiceCollection AddBlazorAppViewModels(this IServiceCollection services)
    {
        return services;
    }
}
