using System;
using ReactiveUI;

namespace Shelly_UI.ViewModels;

public class HomeViewModel  : ViewModelBase, IRoutableViewModel
{
    
    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    public HomeViewModel(IScreen screen) => HostScreen = screen;
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
}