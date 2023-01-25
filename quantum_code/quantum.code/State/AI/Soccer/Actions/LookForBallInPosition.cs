using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe class LookForBallPositionAction : AIAction
    {
        public override unsafe void Update(Frame f, EntityRef e)
        {
            // var fields = f.Get<CharacterFields>(e);
            // var position = f.Get<Transform2D>(e).Position;
            // FPVector2 target = SoccerAIHelper.GetTargetPosition(f, fields.InitialPosition, FPVector2.Zero, fields.Player);
            // SoccerAIHelper.MoveCondition(f, e, position, target, 14);
        }
    }
}