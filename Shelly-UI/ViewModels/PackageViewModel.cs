using System;
using ReactiveUI;

namespace Shelly_UI.ViewModels;

public class PackageViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public PackageViewModel(IScreen screen) => HostScreen = screen;
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
}