using System;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  public unsafe class GoToPositionAction : AIAction
  {
    public override unsafe void Update(Frame f, EntityRef e)
    {
      var fields = f.Get<CharacterFields>(e);
      FPVector2 target = SoccerAIHelper.GetTargetPosition(f, fields.InitialPosition, FPVector2.Right, fields.Player);
      var position = f.Get<Transform2D>(e).Position;
      SoccerAIHelper.SetPositionTarget(f, e, position, target);
    }

  }
}
