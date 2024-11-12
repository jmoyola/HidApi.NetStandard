using System;
using System.Runtime.InteropServices;
using WCharT;

namespace HidApi
{
    public class DeviceInfo{
        private readonly string _path;
        private readonly ushort _vendorId;
        private readonly ushort _productId;
        private readonly string _serialNumber;
        private readonly ushort _releaseNumber;
        private readonly string _manufacturerString;
        private readonly string _productString;
        private readonly ushort _usagePage;
        private readonly ushort _usage;
        private readonly int _interfaceNumber;
        private readonly BusType _busType;

        /// <summary>
        /// Describes a HID device.
        /// </summary>
        /// <param name="Path">Path of the device</param>
        /// <param name="VendorId">Vendor id</param>
        /// <param name="ProductId">Product id</param>
        /// <param name="SerialNumber">Serial Number</param>
        /// <param name="ReleaseNumber">Release number</param>
        /// <param name="ManufacturerString">Manufacturer string</param>
        /// <param name="ProductString">Product string</param>
        /// <param name="UsagePage">Usage page</param>
        /// <param name="Usage">Usage</param>
        /// <param name="InterfaceNumber">interface number</param>
        /// <param name="BusType"><see cref="BusType"/> (Available since hidapi 0.13.0)</param>
        public DeviceInfo(
            string Path,
            ushort VendorId,
            ushort ProductId,
            string SerialNumber,
            ushort ReleaseNumber,
            string ManufacturerString,
            string ProductString,
            ushort UsagePage,
            ushort Usage,
            int InterfaceNumber,
            BusType BusType
        )
        {
            _path = Path;
            _vendorId = VendorId;
            _productId = ProductId;
            _serialNumber = SerialNumber;
            _releaseNumber = ReleaseNumber;
            _manufacturerString = ManufacturerString;
            _productString = ProductString;
            _usagePage = UsagePage;
            _usage = Usage;
            _interfaceNumber = InterfaceNumber;
            _busType = BusType;
        }

        public string Path => _path;
        public ushort VendorId => _vendorId;
        public ushort ProductId=> _productId;
        public string SerialNumber => _serialNumber;
        public ushort ReleaseNumber => _releaseNumber;
        public string ManufacturerString => _manufacturerString;
        public string ProductString => _productString;
        public ushort UsagePage => _usagePage;
        public ushort Usage => _usage;
        public int InterfaceNumber => _interfaceNumber;
        public BusType BusType => _busType;
        
        /// <summary>
        /// Connects to the device defined by the 'Path' property.
        /// </summary>
        /// <returns>A new <see cref="Device"/></returns>
        public Device ConnectToDevice()
        {
            return new Device(Path);
        }

        internal static unsafe DeviceInfo From(NativeDeviceInfo* nativeDeviceInfo)
        {
            return new DeviceInfo(
                Path: Marshal.PtrToStringAnsi((IntPtr) nativeDeviceInfo->Path) ?? string.Empty
                , VendorId: nativeDeviceInfo->VendorId
                , ProductId: nativeDeviceInfo->ProductId
                , SerialNumber: new WCharTString(nativeDeviceInfo->SerialNumber).GetString()
                , ReleaseNumber: nativeDeviceInfo->ReleaseNumber
                , ManufacturerString: new WCharTString(nativeDeviceInfo->ManufacturerString).GetString()
                , ProductString: new WCharTString(nativeDeviceInfo->ProductString).GetString()
                , UsagePage: nativeDeviceInfo->UsagePage
                , Usage: nativeDeviceInfo->Usage
                , InterfaceNumber: nativeDeviceInfo->InterfaceNumber
                , BusType: nativeDeviceInfo->BusType
            );
        }
    }
}
