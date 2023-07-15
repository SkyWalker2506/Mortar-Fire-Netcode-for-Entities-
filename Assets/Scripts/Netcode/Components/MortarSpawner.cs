using Netcode.Blob;
using Unity.Entities;

namespace Netcode.ComponentData
{
    public struct MortarSpawner : IComponentData
    {
        public Entity MortarPrefab;
        public bool DoCreateMortar;
    }
    
}