using Netcode.ComponentData;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Netcode.Baker
{
    public class MortarAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        public float RotateSpeed;
        public float2 AimLimit;
        public float FireInterval;
    }
    
    public class MortarBaker : Baker<MortarAuthoring>
    {
        public override void Bake(MortarAuthoring authoring)
        {
            Entity mortarEntity = GetEntity(TransformUsageFlags.Dynamic);
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