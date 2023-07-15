using Netcode.ComponentData;
using Unity.Entities;
using UnityEngine;

namespace Netcode.Authoring
{
    public class MortarSpawnerAuthoring : MonoBehaviour
    {
        public GameObject MortarPrefab;
        
        class Baking : Baker<MortarSpawnerAuthoring>
        {
            public override void Bake(MortarSpawnerAuthoring authoring)
            {
                Entity mortarSpawnerEntity = GetEntity(TransformUsageFlags.None);
                MortarSpawner mortarSpawner = new MortarSpawner
                {
                    MortarPrefab = GetEntity(authoring.MortarPrefab,TransformUsageFlags.Dynamic),
                    DoCreateMortar = true
                };
                AddComponent(mortarSpawnerEntity, mortarSpawner);
            }
        }
    }
}