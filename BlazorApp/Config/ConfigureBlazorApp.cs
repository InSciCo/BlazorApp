﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.FluentValidation;
using BlazorApp.ViewModels;

namespace BlazorApp;

public static class ConfigureBlazorApp
{
    public static IServiceCollection AddBlazorApp(this IServiceCollection services)
    {
        return services
            .AddBlazorAppViewModels()
            .AddBlazorise(options => { options.Immediate = true; })
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons()
            .AddBlazoriseFluentValidation();
    }
}
