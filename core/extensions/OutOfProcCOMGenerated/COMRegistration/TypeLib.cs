using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace COMRegistration
{
    internal static partial class TypeLib
    {
        public static void Register(string tlbPath)
        {
            Trace.WriteLine($"Registering type library:");
            Trace.Indent();
            Trace.WriteLine(tlbPath);
            Trace.Unindent();

            int hr = OleAut32.LoadTypeLibEx(tlbPath, OleAut32.REGKIND.REGKIND_REGISTER, out ITypeLib _);
            if (hr < 0)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }

        public static void Unregister(string tlbPath)
        {
            Trace.WriteLine($"Unregistering type library:");
            Trace.Indent();
            Trace.WriteLine(tlbPath);
            Trace.Unindent();

            ITypeLib typeLib;
            int hr = OleAut32.LoadTypeLibEx(tlbPath, OleAut32.REGKIND.REGKIND_NONE, out typeLib);
            if (hr < 0)
            {
                Trace.WriteLine($"Unregistering type library failed: 0x{hr:x}");
                return;
            }

            IntPtr attrPtr = IntPtr.Zero;
            try
            {
                typeLib.GetLibAttr(out attrPtr);
                if (attrPtr != IntPtr.Zero)
                {
                    ComTypes.TYPELIBATTR attr = Marshal.PtrToStructure<ComTypes.TYPELIBATTR>(attrPtr);
                    hr = OleAut32.UnRegisterTypeLib(ref attr.guid, attr.wMajorVerNum, attr.wMinorVerNum, attr.lcid, attr.syskind);
                    if (hr < 0)
                    {
                        Trace.WriteLine($"Unregistering type library failed: 0x{hr:x}");
                    }
                }
            }
            finally
            {
                if (attrPtr != IntPtr.Zero)
                {
                    typeLib.ReleaseTLibAttr(attrPtr);
                }
            }
        }

        private partial class OleAut32
        {
            // https://docs.microsoft.com/windows/api/oleauto/ne-oleauto-regkind
            public enum REGKIND
            {
                REGKIND_DEFAULT = 0,
                REGKIND_REGISTER = 1,
                REGKIND_NONE = 2
            }

            // https://docs.microsoft.com/windows/api/oleauto/nf-oleauto-loadtypelibex
            [LibraryImport(nameof(OleAut32), StringMarshalling = StringMarshalling.Utf16)]
            public static partial int LoadTypeLibEx(
                [MarshalAs(UnmanagedType.LPWStr)] string fileName,
                REGKIND regKind,
                out ITypeLib typeLib);

            // https://docs.microsoft.com/windows/api/oleauto/nf-oleauto-unregistertypelib
            [LibraryImport(nameof(OleAut32))]
            public static partial int UnRegisterTypeLib(
                ref Guid id,
                short majorVersion,
                short minorVersion,
                int lcid,
                ComTypes.SYSKIND sysKind);
        }
    }
}
