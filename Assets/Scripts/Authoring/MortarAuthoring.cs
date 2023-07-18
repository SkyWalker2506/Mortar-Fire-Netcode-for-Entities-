using Netcode.ComponentData;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Netcode.Authoring
{
    public class MortarAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        public float RotateSpeed;
        public float3 MinAim;
        public float3 MaxAim;
        public float FireInterval;
        
        class Baking : Baker<MortarAuthoring>
        {
            public override void Bake(MortarAuthoring authoring)
            {
                Entity mortarEntity = GetEntity(TransformUsageFlags.Dynamic);
                Mortar mortar = new Mortar
                {
                    MoveSpeed = authoring.MoveSpeed,
                    RotateSpeed = authoring.RotateSpeed,
                    MinAim = quaternion.Euler(authoring.MinAim),
                    MaxAim = quaternion.Euler(authoring.MaxAim),
                    CurrentAimStep = 0.5f,
                    FireInterval = authoring.FireInterval
                };
                AddComponent(mortarEntity, mortar);
            }
        }
    }
}