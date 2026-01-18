using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using ReactiveUI;
using ReactiveUI.Avalonia;
using Shelly_UI.Models;
using Shelly_UI.ViewModels;

namespace Shelly_UI.Views;

public partial class UpdateWindow : ReactiveUserControl<UpdateViewModel>
{
    public UpdateWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
    
    private void DataGrid_DoubleTapped(object? sender, TappedEventArgs e)
    {
        var row = (e.Source as Visual)?.FindAncestorOfType<DataGridRow>();

        if (row?.DataContext is not UpdateModel package) return;
        if (DataContext is not UpdateViewModel vm) return;


        vm.TogglePackageCheckCommand.Execute(package).Subscribe();
    }
}