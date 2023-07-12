using Netcode.Blob;
using Unity.Entities;

namespace Netcode.ComponentData
{
    public struct MortarSpawnPoints : IComponentData
    {
        public BlobAssetReference<TransformBlobArray> Value;
    }
}