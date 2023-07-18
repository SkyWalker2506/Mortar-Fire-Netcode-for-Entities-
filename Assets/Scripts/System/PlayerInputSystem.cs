using Netcode.ComponentData.Input;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Netcode.System
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial struct PlayerInputSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerInput>();
            state.RequireForUpdate<NetworkId>();
        }

        public void OnUpdate(ref SystemState state)
        {
            bool left = Input.GetKey(KeyCode.LeftArrow);
            bool right = Input.GetKey(KeyCode.RightArrow);
            bool up = Input.GetKey(KeyCode.UpArrow);
            bool down = Input.GetKey(KeyCode.DownArrow);
            bool fire = Input.GetKey(KeyCode.Space);
            foreach (RefRW<PlayerInput> input in SystemAPI.Query<RefRW<PlayerInput>>().WithAll<GhostOwnerIsLocal>())
            {
                input.ValueRW = default;
                if (left)
                {
                    input.ValueRW.MovementDirection -= 1;
                }
                if (right)
                {
                    input.ValueRW.MovementDirection += 1;

                }
                if (up)
                {
                    input.ValueRW.Aim -= 1;
                }
                if (down)
                {
                    input.ValueRW.Aim += 1;

                }
                if (fire)
                {
                    input.ValueRW.Fire.Set();
                }
            }
        }
    }
   
}