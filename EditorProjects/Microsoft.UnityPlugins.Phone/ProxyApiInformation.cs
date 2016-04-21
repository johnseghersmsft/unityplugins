namespace Microsoft.UnityPlugins
{
    public class ProxyApiInformation
    {
        public static bool IsPhone { get { return false; } }
        public static bool IsTypePresent(string typename)
        {
            return false;
        }

        public static bool IsApiContractPresent(string contractName, ushort majorVersion)
        {
            return false;
        }

        public static bool IsApiContractPresent(string contractName, ushort majorVersion, ushort minorVersion)
        {
            return false;
        }
    }
}
