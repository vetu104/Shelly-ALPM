using System;
using ReactiveUI;

namespace Shelly_UI.ViewModels;

public class RemoveViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public RemoveViewModel(IScreen screen) => HostScreen = screen;
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
}