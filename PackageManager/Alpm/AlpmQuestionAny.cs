using System.Runtime.InteropServices;

namespace PackageManager.Alpm;

[StructLayout(LayoutKind.Sequential)]
public struct AlpmQuestionAny
{
    public int Type;
    public int Answer;
}