using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

[ComVisible(true)]
[Guid(Contract.Constants.ServerInterface)]
[GeneratedComInterface]
public partial interface IServer
{
    /// <summary>
    /// Compute the value of the constant Pi.
    /// </summary>
    double ComputePi();
}
