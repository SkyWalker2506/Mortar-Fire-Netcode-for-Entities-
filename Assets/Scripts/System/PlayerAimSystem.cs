using Netcode.ComponentData;
using Netcode.ComponentData.Input;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Netcode.System
{
    
    public struct RotateMortarRequest : IRpcCommand
    {
        public float AimStep;
    }

    
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct PlayerAimClientSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkStreamInGame>();
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<Mortar>()
                .WithAll<PlayerInput>();
            
            state.RequireForUpdate(state.GetEntityQuery(builder));
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach ((RefRW<Mortar> mortar, RefRO<PlayerInput> input, Entity entity) in SystemAPI.Query<RefRW<Mortar>,RefRO<PlayerInput>>()
                         .WithEntityAccess().WithAll<Simulate>().WithAll<GhostOwnerIsLocal>())
            {
                Debug.Log(mortar.ValueRW.CurrentAimStep);
                float rotateDirection = -input.ValueRO.Aim;
                if (rotateDirection == 0)
                {
                    return;
                }
                float rotateSpeed = mortar.ValueRO.RotateSpeed * SystemAPI.Time.DeltaTime;
                mortar.ValueRW.CurrentAimStep += rotateDirection * rotateSpeed*.1f;
                Entity req = commandBuffer.CreateEntity();
                commandBuffer.AddComponent(req,new RotateMortarRequest{AimStep = mortar.ValueRW.CurrentAimStep });
                commandBuffer.AddComponent(req, new SendRpcCommandRequest ());
                Debug.Log("CommentSent "+ mortar.ValueRW.CurrentAimStep );
            }
            commandBuffer.Playback(state.EntityManager);

        }
    }
    
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct PlayerAimSeverSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<Mortar>()
                .WithAll<PlayerInput>();
            
            state.RequireForUpdate(state.GetEntityQuery(builder));
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);

            commandBuffer.Playback(state.EntityManager);

        }
    }
}

