using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Netcode.ComponentData
{
    public struct Mortar : IComponentData
    {
        public float MoveSpeed;
        public float RotateSpeed;
        public Quaternion MinAim; 
        public Quaternion MaxAim;
        public float CurrentAimStep;
        public float FireInterval;
    }
}
