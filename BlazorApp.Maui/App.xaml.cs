﻿using Microsoft.Maui.Controls;

namespace BlazorApp.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}
