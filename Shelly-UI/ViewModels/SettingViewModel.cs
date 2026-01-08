using System;
using ReactiveUI;

namespace Shelly_UI.ViewModels;

public class SettingViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public SettingViewModel(IScreen screen) => HostScreen = screen;
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
}