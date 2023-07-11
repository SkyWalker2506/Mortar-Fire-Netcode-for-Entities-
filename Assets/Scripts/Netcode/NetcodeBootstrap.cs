using Unity.NetCode;
using UnityEngine.Scripting;

namespace Netcode.Bootstrap
{
    [Preserve]
    public class NetcodeBootstrap : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName)
        {
            AutoConnectPort = 7777;
            return base.Initialize(defaultWorldName);
        }
    }
}
