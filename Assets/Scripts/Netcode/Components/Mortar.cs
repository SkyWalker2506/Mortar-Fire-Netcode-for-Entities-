using Unity.Entities;
using Unity.Mathematics;

namespace Netcode.ComponentData
{
    public struct Mortar : IComponentData
    {
        public float MoveSpeed;
        public float RotateSpeed;
        public float2 AimLimit;
        public float FireInterval;
    }
}
