using Avalonia.Markup.Xaml;
using ReactiveUI;
using ReactiveUI.Avalonia;
using Shelly_UI.ViewModels;

namespace Shelly_UI.Views;

public partial class UpdateWindow : ReactiveUserControl<UpdateViewModel>
{
    public UpdateWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}