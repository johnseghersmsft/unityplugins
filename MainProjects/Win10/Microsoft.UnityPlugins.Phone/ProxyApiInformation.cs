using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.UnityPlugins
{
    public class ProxyApiInformation
    {
        public static bool IsPhone
        {
            get
            {
                if (_isPhone == null)
                {
                    _isPhone = IsApiContractPresent("Windows.Phone.PhoneContract", 1);
                }
                return _isPhone.Value;
            }
        }
        public static bool IsTypePresent(string typename)
        {
            return Windows.Foundation.Metadata.ApiInformation.IsTypePresent(typename);
        }

        public static bool IsApiContractPresent(string contractName, ushort majorVersion)
        {
            return Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent(contractName, majorVersion);
        }

        public static bool IsApiContractPresent(string contractName, ushort majorVersion, ushort minorVersion)
        {
            return Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent(contractName, majorVersion, minorVersion);
        }

        private static bool? _isPhone;
    }
}
