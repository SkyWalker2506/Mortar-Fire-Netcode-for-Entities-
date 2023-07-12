using Unity.Entities;
using Unity.Mathematics;

namespace Netcode.Blob
{
    public struct TransformBlobArray
    {
        public BlobArray<TransformData> Transforms;
    }
    
    public struct TransformData
    {
        public float3 Position;
        public float3 Rotation;
    } 
}