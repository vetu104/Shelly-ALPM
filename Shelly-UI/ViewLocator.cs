using System;
using System.Diagnostics.CodeAnalysis;
using ReactiveUI;
using Shelly_UI.ViewModels;
using Shelly_UI.Views;

namespace Shelly_UI;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        HomeViewModel context => new HomeWindow() { DataContext = context },
        SettingViewModel context => new SettingWindow() { DataContext = context },
        UpdateViewModel context => new UpdateWindow() { DataContext = context },
        PackageViewModel context => new PackageWindow() { DataContext = context },
        RemoveViewModel context => new RemoveWindow() { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}