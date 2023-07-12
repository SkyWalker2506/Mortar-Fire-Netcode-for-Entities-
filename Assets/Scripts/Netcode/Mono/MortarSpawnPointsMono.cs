using Netcode.Blob;
using Netcode.ComponentData;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Netcode.Baker
{
    public class MortarSpawnPointsMono : MonoBehaviour
    {
        public Transform[] MortarSpawnTransforms;
        
        public class MortarSpawnerBaker : Baker<MortarSpawnPointsMono>
        {
            public override void Bake(MortarSpawnPointsMono authoring)
            {
                Entity mortarSpawnPointsEntity = GetEntity(TransformUsageFlags.None);
                var builder = new BlobBuilder(Allocator.Temp);
                ref var spawnPoints = ref builder.ConstructRoot<TransformBlobArray>();
                BlobBuilderArray<TransformData> blobBuilder = builder.Allocate(ref spawnPoints.Transforms, authoring.MortarSpawnTransforms.Length);
                
                for (var i = 0; i < authoring.MortarSpawnTransforms.Length; i++)
                {
                    TransformData newSpawnPoint = new TransformData
                    {
                        Position = authoring.MortarSpawnTransforms[i].position,
                        Rotation =  authoring.MortarSpawnTransforms[i].localRotation.eulerAngles
                    };
                    blobBuilder[i] = newSpawnPoint;
                }
                
                BlobAssetReference<TransformBlobArray> blobAsset = builder.CreateBlobAssetReference<TransformBlobArray>(Allocator.Persistent);
                SetComponent(mortarSpawnPointsEntity, new MortarSpawnPoints{Value = blobAsset});
                builder.Dispose();
            }
        }
    }
}

