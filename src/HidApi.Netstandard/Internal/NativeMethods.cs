using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using NativeLibraryNetStandard;
using WCharT;

namespace HidApi
{

    internal static class NativeMethods
    {
        private delegate int hidInit();
        private delegate int hidExit();
        private unsafe delegate NativeDeviceInfo* hidEnumerate(ushort vendorId, ushort productId);
        private unsafe delegate void hidFreeEnumeration(NativeDeviceInfo* devices); 
        private unsafe delegate DeviceSafeHandle hidOpen(ushort vendorId, ushort productId, byte* serialNumber);
        private delegate DeviceSafeHandle hidOpenPath([MarshalAs(UnmanagedType.LPStr)] string path); 
        private delegate int hidWrite(DeviceSafeHandle device, ref byte data, nuint length);
        private delegate int hidReadTimeOut(DeviceSafeHandle device, ref byte data, nuint length, int milliseconds); 
        private delegate int hidRead(DeviceSafeHandle device, ref byte data, nuint length);
        private delegate int hidSetNonBlocking(DeviceSafeHandle device, int nonBlock);
        private delegate int hidSendFeatureReport(DeviceSafeHandle device, ref byte data, nuint length);
        private delegate int hidGetFeatureReport(DeviceSafeHandle device, ref byte data, nuint length);
        private delegate int hidGetInputReport(DeviceSafeHandle device, ref byte data, nuint length);
        private delegate void hidClose(IntPtr device);
        private delegate int hidGetManufacturerString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);
        private delegate int hidGetProductString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);
        private delegate int hidGetSerialNumberString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);
        private unsafe delegate NativeDeviceInfo* hidGetDeviceInfo(DeviceSafeHandle device);
        private delegate int hidGetIndexedString(DeviceSafeHandle device, int stringIndex, ref byte buffer, nuint maxLength);
        private delegate int hidGetReportDescriptor(DeviceSafeHandle device, ref byte buf, nuint bufSize);
        private unsafe delegate byte* hidError(DeviceSafeHandle device);
        private unsafe delegate ApiVersion* hidVersion();
        private delegate IntPtr hidVersionString();
        
        
        
        
        private static NativeLibraryHolder hidApi;

        static NativeMethods()
        {
            
            string[] names=NativeHidApiLibrary.GetNames().ToArray();
            Debug.WriteLine(string.Join(", ",names));
            
            NativeLibraryLoader loader = NativeLibraryLoader.GetPlatformDefaultLoader();
            IPathResolver pResolver = new DefaultNetAppPathResolver();
             hidApi=new NativeLibraryHolder(names, loader, pResolver, true);
        }

        public static unsafe DeviceSafeHandle Open(ushort vendorId, ushort productId, WCharTString serialNumber)
        {
            fixed (byte* ptr = serialNumber)
            {
                return Open(vendorId, productId, ptr);
            }
        }

        public static int Write(DeviceSafeHandle device, ReadOnlySpan<byte> data)
        {
            return Write(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
        }

        public static int ReadTimeOut(DeviceSafeHandle device, Span<byte> data, int milliseconds)
        {
            return ReadTimeOut(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length, milliseconds);
        }

        public static int Read(DeviceSafeHandle device, Span<byte> data)
        {
            return Read(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
        }

        public static int GetManufacturerString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer, int maxLength)
        {
            return GetManufacturerString(device, ref MemoryMarshal.GetReference(buffer), (nuint) maxLength);
        }

        public static int GetProductString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer, int maxLength)
        {
            return GetProductString(device, ref MemoryMarshal.GetReference(buffer), (nuint) maxLength);
        }

        public static int GetSerialNumberString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer, int maxLength)
        {
            return GetSerialNumberString(device, ref MemoryMarshal.GetReference(buffer), (nuint) maxLength);
        }

        public static int GetIndexedString(DeviceSafeHandle device, int index, ReadOnlySpan<byte> buffer, int maxLength)
        {
            return GetIndexedString(device, index, ref MemoryMarshal.GetReference(buffer), (nuint) maxLength);
        }

        public static int SendFeatureReport(DeviceSafeHandle device, ReadOnlySpan<byte> data)
        {
            return SendFeatureReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
        }

        public static int GetFeatureReport(DeviceSafeHandle device, Span<byte> data)
        {
            return GetFeatureReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
        }

        public static int GetInputReport(DeviceSafeHandle device, Span<byte> data)
        {
            return GetInputReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
        }

        public static int GetReportDescriptor(DeviceSafeHandle device, Span<byte> buf)
        {
            return GetReportDescriptor(device, ref MemoryMarshal.GetReference(buf), (nuint) buf.Length);
        }
        
        public static int Init()
        {
            return hidApi.LoadFunction<hidInit>("hid_init").Invoke();
        }
        
        public static int Exit()
        {
            return hidApi.LoadFunction<hidExit>("hid_exit").Invoke();
        }
        
        public static unsafe NativeDeviceInfo* Enumerate(ushort vendorId, ushort productId)
        {
            return hidApi.LoadFunction<hidEnumerate>("hid_enumerate").Invoke(vendorId, productId);
        }
        
        public static unsafe void FreeEnumeration(NativeDeviceInfo* devices)
        {
            hidApi.LoadFunction<hidFreeEnumeration>("hid_free_enumeration").Invoke(devices);
        }
        
        private static unsafe DeviceSafeHandle Open(ushort vendorId, ushort productId, byte* serialNumber)
        {
            return hidApi.LoadFunction<hidOpen>("hid_open").Invoke(vendorId, productId, serialNumber);
        }
        
        public static DeviceSafeHandle OpenPath([MarshalAs(UnmanagedType.LPStr)] string path)
        {
            return hidApi.LoadFunction<hidOpenPath>("hid_open_path").Invoke(path);
        }
        
        private static int Write(DeviceSafeHandle device, ref byte data, nuint length)
        {
            return hidApi.LoadFunction<hidWrite>("hid_write").Invoke(device, ref data, length);
        }
        
        private static int ReadTimeOut(DeviceSafeHandle device, ref byte data, nuint length, int milliseconds)
        {
            return hidApi.LoadFunction<hidReadTimeOut>("hid_read_timeout").Invoke(device, ref data, length, milliseconds);
        }
        
        private static int Read(DeviceSafeHandle device, ref byte data, nuint length)
        {
            return hidApi.LoadFunction<hidRead>("hid_read").Invoke(device, ref data, length);
        }
        
        public static int SetNonBlocking(DeviceSafeHandle device, int nonBlock)
        {
            return hidApi.LoadFunction<hidSetNonBlocking>("hid_set_nonblocking").Invoke(device, nonBlock);
        }
        
        private static int SendFeatureReport(DeviceSafeHandle device, ref byte data, nuint length)
        {
            return hidApi.LoadFunction<hidSendFeatureReport>("hid_send_feature_report").Invoke(device, ref data, length);
        }
        
        private static int GetFeatureReport(DeviceSafeHandle device, ref byte data, nuint length)
        {
            return hidApi.LoadFunction<hidGetFeatureReport>("hid_get_feature_report").Invoke(device, ref data, length);
        }
        
        private static int GetInputReport(DeviceSafeHandle device, ref byte data, nuint length)
        {
            return hidApi.LoadFunction<hidGetInputReport>("hid_get_input_report").Invoke(device, ref data, length);
        }
        
        public static void Close(IntPtr device)
        {
            hidApi.LoadFunction<hidClose>("hid_close").Invoke(device);
        }
        
        private static int GetManufacturerString(DeviceSafeHandle device, ref byte buffer, nuint maxLength)
        {
            return hidApi.LoadFunction<hidGetManufacturerString>("hid_get_manufacturer_string").Invoke(device, ref buffer, maxLength);
        }
        
        private static int GetProductString(DeviceSafeHandle device, ref byte buffer, nuint maxLength)
        {
            return hidApi.LoadFunction<hidGetProductString>("hid_get_product_string").Invoke(device, ref buffer, maxLength);
        }
        
        private static int GetSerialNumberString(DeviceSafeHandle device, ref byte buffer, nuint maxLength)
        {
            return hidApi.LoadFunction<hidGetSerialNumberString>("hid_get_serial_number_string").Invoke(device, ref buffer, maxLength);
        }
        
        public static unsafe NativeDeviceInfo* GetDeviceInfo(DeviceSafeHandle device)
        {
            return hidApi.LoadFunction<hidGetDeviceInfo>("hid_get_device_info").Invoke(device);    
        }
    
        private static int GetIndexedString(DeviceSafeHandle device, int stringIndex, ref byte buffer,
            nuint maxLength)
        {
            return hidApi.LoadFunction<hidGetIndexedString>("hid_get_indexed_string").Invoke(device, stringIndex, ref buffer, maxLength);
        }
        
        private static int GetReportDescriptor(DeviceSafeHandle device, ref byte buf, nuint bufSize)
        {
            return hidApi.LoadFunction<hidGetReportDescriptor>("hid_get_report_descriptor").Invoke(device, ref buf, bufSize);
        }
        
        public static unsafe byte* Error(DeviceSafeHandle device)
        {
            return hidApi.LoadFunction<hidError>("hid_error").Invoke(device);
        }
        
        public static unsafe ApiVersion* Version()
        {
            return hidApi.LoadFunction<hidVersion>("hid_version").Invoke();
        }
        
        public static IntPtr VersionString()
        {
            return hidApi.LoadFunction<hidVersionString>("hid_version_str").Invoke();
        }

    }
}
