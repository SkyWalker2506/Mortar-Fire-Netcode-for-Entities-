using Netcode.ComponentData;
using Netcode.ComponentData.Input;
using Unity.Entities;
using UnityEngine;

namespace Netcode.Authoring
{
    public class PlayerInputAuthoring : MonoBehaviour
    {
        class Baking : Baker<PlayerInputAuthoring>
        {
            public override void Bake(PlayerInputAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None),  new PlayerInput());
            }
        }
    }
    
    
}