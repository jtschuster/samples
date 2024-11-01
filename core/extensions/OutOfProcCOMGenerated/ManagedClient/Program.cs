using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: DisableRuntimeMarshalling]
namespace ManagedClient
{
    partial class Program
    {
        static void Main(string[] _)
        {
            // If the COM server is registered as an in-proc server (as is the case when using
            // a DLL surrogate), activation through the Activator will only use the out-of-proc
            // server if the client and the registered COM server are not the same bitness.
            //
            // Type t = Type.GetTypeFromCLSID(Contract.Constants.ServerClassGuid);
            // var server = (IServer)Activator.CreateInstance(t);
            //
            // This demo explicitly calls CoCreateInstance with CLSCTX_LOCAL_SERVER to force
            // usage of the out-of-proc server.
            object obj;
            int hr = Ole32.CoCreateInstance(Contract.Constants.ServerClassGuid, IntPtr.Zero, Ole32.CLSCTX_LOCAL_SERVER, typeof(IServer).GUID, out obj);
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }

            var server = (IServer)obj;
            double pi = server.ComputePi();
            Console.WriteLine($"\u03C0 = {pi}");
        }

        private partial class Ole32
        {
            // https://docs.microsoft.com/windows/win32/api/wtypesbase/ne-wtypesbase-clsctx
            public const int CLSCTX_LOCAL_SERVER = 0x4;

            // https://docs.microsoft.com/windows/win32/api/combaseapi/nf-combaseapi-cocreateinstance
            [LibraryImport(nameof(Ole32))]
            public static partial int CoCreateInstance(
                Guid rclsid,
                IntPtr pUnkOuter,
                uint dwClsContext,
                Guid riid,
                [MarshalAs(UnmanagedType.Interface)] out object ppv);
        }
    }
}
