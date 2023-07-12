using Netcode.ComponentData;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Netcode.Baker
{
    
    namespace Netcode.Baker
    {
        public class MortarSpawnerMono : MonoBehaviour
        {
            public GameObject MortarPrefab;
            public Transform[] MortarSpawnTransforms;
        }
    
        public class MortarSpawnerBaker : Baker<MortarSpawnerMono>
        {
            public override void Bake(MortarSpawnerMono authoring)
            {
                Entity mortarSpawnerEntity = GetEntity(TransformUsageFlags.Dynamic);
                Mortar mortar = new Mortar
                {
                    MoveSpeed = authoring.MoveSpeed,
                    RotateSpeed = authoring.RotateSpeed,
                    AimLimit = authoring.AimLimit,
                    FireInterval = authoring.FireInterval
                };
                AddComponent(mortarEntity, mortar);
            }
        }
    }
   
}