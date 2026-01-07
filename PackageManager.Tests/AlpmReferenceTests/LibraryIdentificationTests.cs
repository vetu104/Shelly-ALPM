namespace PackageManager.Tests.AlpmReferenceTests;

public class LibraryIdentificationTests
{
    [Test]
    public void SuccessfullyResolvesAlpmLibrary()
    {
        IntPtr handle = Alpm.AlpmReference.ResolveAlpm(Alpm.AlpmReference.LibName, typeof(Alpm.AlpmReference).Assembly, null);
        Assert.That(handle, Is.Not.EqualTo(IntPtr.Zero));
    }
    
}