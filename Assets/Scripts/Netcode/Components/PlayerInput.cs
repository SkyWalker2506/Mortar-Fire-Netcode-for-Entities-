using Unity.NetCode;

namespace Netcode.ComponentData.Input
{
    [GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
    public struct PlayerInput : IInputComponentData
    {
        public int MovementDirection;
        public int Aim;
        public InputEvent Fire;
    }
}
