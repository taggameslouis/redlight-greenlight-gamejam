using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe class DefendPositionAction : AIAction
    {
        public override unsafe void Update(Frame f, EntityRef e)
        {
            // var fields = f.Get<CharacterFields>(e);
            // var position = f.Get<Transform2D>(e).Position;
            // FPVector2 target = SoccerAIHelper.GetTargetPosition(f, fields.InitialPosition, FPVector2.Right, fields.Player);
            // SoccerAIHelper.MoveCondition(f, e, position, target, 10);
        }
    }
}