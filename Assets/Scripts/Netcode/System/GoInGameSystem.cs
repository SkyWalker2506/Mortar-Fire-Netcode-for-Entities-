using Netcode.RpcCommand;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Netcode.System
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct GoInGameClientSystem : ISystem 
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<NetworkId>()
                .WithNone<NetworkStreamInGame>();
            state.RequireForUpdate(state.GetEntityQuery(builder));
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach ((RefRO<NetworkId> id, Entity entity) in SystemAPI.Query<RefRO<NetworkId>>().WithEntityAccess().WithNone<NetworkStreamInGame>())
            {
                commandBuffer.AddComponent<NetworkStreamInGame>(entity);
                Entity req = commandBuffer.CreateEntity();
                commandBuffer.AddComponent<GoInGameRPC>(req);
                commandBuffer.AddComponent(req, new SendRpcCommandRequest{ TargetConnection = entity });
            }
            commandBuffer.Playback(state.EntityManager);
        }
    }
    
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct GoInGameServerSystem : ISystem
    {
        public ComponentLookup<NetworkId> NetworkId;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<GoInGameRPC>()
                .WithAll<ReceiveRpcCommandRequest>();
            state.RequireForUpdate(state.GetEntityQuery(builder));
            NetworkId = state.GetComponentLookup<NetworkId>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //FixedString128Bytes worldName = state.WorldUnmanaged.Name;
            EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            NetworkId.Update(ref state);

            foreach ((RefRO<ReceiveRpcCommandRequest> req, Entity reqEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRPC>().WithEntityAccess())
            {
                commandBuffer.AddComponent<NetworkStreamInGame>(req.ValueRO.SourceConnection);
                NetworkId networkId = NetworkId[req.ValueRO.SourceConnection];
              //  Debug.Log(worldName + " connecting "+networkId.Value);
                commandBuffer.DestroyEntity(reqEntity);
            }
            
            commandBuffer.Playback(state.EntityManager);
        }
    }
}