using Netcode.ComponentData;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Netcode.System
{
    
    public struct SpawnMortarRequest : IRpcCommand
    {
    }

    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct MortarSpawnerClientSystem : ISystem
    {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MortarSpawner>();
            state.RequireForUpdate<NetworkStreamInGame>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach (var spawner in SystemAPI.Query<RefRW<MortarSpawner>>())
            {
                if (!spawner.ValueRO.DoCreateMortar)
                {
                    return;
                }
                spawner.ValueRW.DoCreateMortar = false;
                Entity req = commandBuffer.CreateEntity();
                commandBuffer.AddComponent<SpawnMortarRequest>(req);
                commandBuffer.AddComponent(req, new SendRpcCommandRequest());
            }
            commandBuffer.Playback(state.EntityManager);
        }
    }

    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct MortarSpawnerServerSystem : ISystem
    {
        private ComponentLookup<NetworkId> networkIdFromEntity;
        private int index;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
           state.RequireForUpdate<MortarSpawner>();
           state.RequireForUpdate<MortarSpawnPoints>();
           networkIdFromEntity = state.GetComponentLookup<NetworkId>(true);
           index = 0;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            Entity mortarPrefab = SystemAPI.GetSingleton<MortarSpawner>().MortarPrefab;
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            networkIdFromEntity.Update(ref state);
            ref var spawnPoints = ref SystemAPI.GetSingleton<MortarSpawnPoints>().Value.Value.Transforms;
            foreach ((RefRO<ReceiveRpcCommandRequest> reqSrc,  Entity reqEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>()
                        .WithAll<SpawnMortarRequest>().WithEntityAccess())
            {
                var networkId = networkIdFromEntity[reqSrc.ValueRO.SourceConnection];
                Entity mortar = commandBuffer.Instantiate(mortarPrefab);
                
                commandBuffer.SetComponent(mortar, new GhostOwner { NetworkId = networkId.Value});
                commandBuffer.SetComponent(mortar,new LocalTransform
                {
                    Position = spawnPoints[index].Position,
                    Rotation = Quaternion.Euler(spawnPoints[index].Rotation),
                    Scale = 1
                });
                commandBuffer.AppendToBuffer(reqSrc.ValueRO.SourceConnection, new LinkedEntityGroup{Value = mortar});
                commandBuffer.DestroyEntity(reqEntity);
                index = (index+1)%spawnPoints.Length;
            }
            commandBuffer.Playback(state.EntityManager);

        }
    }
    
}