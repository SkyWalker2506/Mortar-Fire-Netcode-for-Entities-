using Netcode.Blob;
using Netcode.ComponentData;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Hash128 = Unity.Entities.Hash128;

namespace Netcode.Authoring
{
    public class MortarSpawnPointsAuthoring : MonoBehaviour
    {
        public Transform[] MortarSpawnTransforms;
        
        public class Baking : Baker<MortarSpawnPointsAuthoring>
        {
            public override void Bake(MortarSpawnPointsAuthoring authoring)
            {
                Entity mortarSpawnPointsEntity = GetEntity(TransformUsageFlags.None);
                BlobAssetReference<TransformBlobArray> blobAsset;
                using (var builder = new BlobBuilder(Allocator.Temp))
                {
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
                    blobAsset = builder.CreateBlobAssetReference<TransformBlobArray>(Allocator.Persistent);
                }
                
                AddBlobAsset(ref blobAsset, out Hash128 hash);
                AddComponent(mortarSpawnPointsEntity,new MortarSpawnPoints {Value = blobAsset});
            }
        }
    }
}

