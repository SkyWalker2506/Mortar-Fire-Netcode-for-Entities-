using Netcode.ComponentData;
using Netcode.ComponentData.Input;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Netcode.System
{
       [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
       public partial struct PlayerMovementSystem : ISystem
       {
              public void OnCreate(ref SystemState state)
              {
                     EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<Mortar>()
                            .WithAll<PlayerInput>().WithAll<Simulate>();
            
                     state.RequireForUpdate(state.GetEntityQuery(builder));
              }

              public void OnUpdate(ref SystemState state)
              {
                     foreach ((RefRO<Mortar> mortar, RefRO<PlayerInput> input, RefRW<LocalTransform> transform) in SystemAPI.Query<RefRO<Mortar>,RefRO<PlayerInput>,RefRW<LocalTransform>>().WithAll<Simulate>())
                     {
                            if (input.ValueRO.MovementDirection == 0)
                            {
                                   return;
                            }
                            float3 moveDirection = new float3(input.ValueRO.MovementDirection,0,0);
                            float moveSpeed = mortar.ValueRO.MoveSpeed * SystemAPI.Time.DeltaTime;
                            transform.ValueRW.Position += moveSpeed * moveDirection;
                     }
              }
       }
}
