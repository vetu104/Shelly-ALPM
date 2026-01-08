using Avalonia.Markup.Xaml;
using ReactiveUI;
using ReactiveUI.Avalonia;
using Shelly_UI.ViewModels;

namespace Shelly_UI.Views;

public partial class SettingWindow : ReactiveUserControl<SettingViewModel>
{
    public SettingWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}