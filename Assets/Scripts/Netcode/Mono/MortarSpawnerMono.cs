using Netcode.ComponentData;
using Unity.Entities;
using UnityEngine;

namespace Netcode.Baker
{
    
    namespace Netcode.Baker
    {
        public class MortarSpawnerMono : MonoBehaviour
        {
            public GameObject MortarPrefab;
        }
    
        public class MortarSpawnerBaker : Baker<MortarSpawnerMono>
        {
            public override void Bake(MortarSpawnerMono authoring)
            {
                Entity mortarSpawnerEntity = GetEntity(TransformUsageFlags.None);
                MortarSpawner mortarSpawner = new MortarSpawner
                {
                    
                };
                AddComponent(mortarSpawnerEntity, mortarSpawner);
            }
        }
    }
   
}