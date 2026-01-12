using Moq;
using Shelly_UI.ViewModels;
using Shelly_UI.Services;
using Shelly_UI.Services.AppCache;
using PackageManager.Alpm;
using Microsoft.Reactive.Testing;
using System.Reactive.Linq;
using System.Reactive;

namespace Shelly_UI.Tests.ViewModels;

public class MainWindowViewModelTests : TestScheduler
{
    private Mock<IConfigService> _configServiceMock;
    private Mock<IAppCache> _appCacheMock;
    private Mock<IAlpmManager> _alpmManagerMock;

    [SetUp]
    public void Setup()
    {
        _configServiceMock = new Mock<IConfigService>();
        _appCacheMock = new Mock<IAppCache>();
        _alpmManagerMock = new Mock<IAlpmManager>();
    }

    [Test]
    public void IsProcessing_ShouldBeFalse_Initially()
    {
        var vm = new MainWindowViewModel(_configServiceMock.Object, _appCacheMock.Object, _alpmManagerMock.Object, this);
        Assert.That(vm.IsProcessing, Is.False);
    }

    [Test]
    public void IsProcessing_ShouldBeTrue_WhenPackageOperationStartEventOccurs()
    {
        var vm = new MainWindowViewModel(_configServiceMock.Object, _appCacheMock.Object, _alpmManagerMock.Object, this);
        
        _alpmManagerMock.Raise(m => m.PackageOperation += null, 
            new AlpmPackageOperationEventArgs(AlpmEventType.PackageOperationStart, "test-package"));

        Assert.That(vm.IsProcessing, Is.True);
        Assert.That(vm.ProcessingMessage, Contains.Substring("test-package"));
    }

    [Test]
    public void IsProcessing_ShouldBeFalse_AfterTimeout()
    {
        var vm = new MainWindowViewModel(_configServiceMock.Object, _appCacheMock.Object, _alpmManagerMock.Object, this);
        
        _alpmManagerMock.Raise(m => m.PackageOperation += null, 
            new AlpmPackageOperationEventArgs(AlpmEventType.PackageOperationStart, "test-package"));

        Assert.That(vm.IsProcessing, Is.True);
        
        // Advance time by 5 seconds
        AdvanceBy(TimeSpan.FromSeconds(5).Ticks);
        
        Assert.That(vm.IsProcessing, Is.False);
        Assert.That(vm.ProcessingMessage, Is.Empty);
    }

    [Test]
    public void IsProcessing_ShouldNotTimeout_IfNewEventReceived()
    {
        var vm = new MainWindowViewModel(_configServiceMock.Object, _appCacheMock.Object, _alpmManagerMock.Object, this);
        
        _alpmManagerMock.Raise(m => m.PackageOperation += null, 
            new AlpmPackageOperationEventArgs(AlpmEventType.PackageOperationStart, "test-package-1"));

        Assert.That(vm.IsProcessing, Is.True);
        
        // Advance time by 4 seconds
        AdvanceBy(TimeSpan.FromSeconds(4).Ticks);
        Assert.That(vm.IsProcessing, Is.True);
        
        // New event
        _alpmManagerMock.Raise(m => m.PackageOperation += null, 
            new AlpmPackageOperationEventArgs(AlpmEventType.PackageOperationStart, "test-package-2"));
        
        // Advance time by another 4 seconds (total 8 from start, but only 4 from last event)
        AdvanceBy(TimeSpan.FromSeconds(4).Ticks);
        Assert.That(vm.IsProcessing, Is.True);
        
        // Advance time by another 1 second (total 5 from last event)
        AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        Assert.That(vm.IsProcessing, Is.False);
    }

    [Test]
    public void IsProcessing_ShouldBeFalse_WhenPackageOperationDoneEventOccurs()
    {
        var vm = new MainWindowViewModel(_configServiceMock.Object, _appCacheMock.Object, _alpmManagerMock.Object, this);
        
        _alpmManagerMock.Raise(m => m.PackageOperation += null, 
            new AlpmPackageOperationEventArgs(AlpmEventType.PackageOperationStart, "test-package"));
        
        _alpmManagerMock.Raise(m => m.PackageOperation += null, 
            new AlpmPackageOperationEventArgs(AlpmEventType.PackageOperationDone, "test-package"));

        Assert.That(vm.IsProcessing, Is.False);
        Assert.That(vm.ProcessingMessage, Is.Empty);
    }
}
