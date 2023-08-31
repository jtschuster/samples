using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Win32;
using static Tutorial.ComInterfaces;

namespace Tutorial;

[GeneratedComClass]
[Guid(ClsId)]
internal partial class Calculator : ISimpleCalculator
{
    public int Add(int a, int b)
    {
        Console.WriteLine("Adding");
        return a + b;
    }
    public int Subtract(int a, int b) => a - b;
    internal const string ClsId = ClsIds.Calculator;
}
