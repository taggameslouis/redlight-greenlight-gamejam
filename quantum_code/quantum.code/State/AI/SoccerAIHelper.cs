using System;
using Photon.Deterministic;

namespace Quantum
{
  public unsafe class SoccerAIHelper
  {
    public static FPVector2 GetTargetPosition(Frame f, FPVector2 initialPosition, FPVector2 direction, int team)
    {
      var ballPosition = f.Get<Transform2D>(f.Global->Ball).Position;
      FPVector2 target = initialPosition;

      FP offset = ballPosition.X;
      if (direction != FPVector2.Zero)
      {
        offset = direction.X * (int)ballPosition.X;
      }

      if (team == 0)
      {
        offset = FPMath.Clamp(offset, -10, 30);
      }
      else
      {
        offset = FPMath.Clamp(offset, -30, 10);
      }
      target.X += offset;
      target.X = FPMath.Clamp(target.X, -35, 35);
      return target;
    }


    public static void MoveCondition(Frame f, EntityRef character, FPVector2 position, FPVector2 target, FP maxDistance)
    {
      var ballTransform = f.Get<Transform2D>(f.Global->Ball);
      if (FPVector2.DistanceSquared(target, ballTransform.Position) < maxDistance * maxDistance)
      {
        SetPositionTarget(f, character, position, ballTransform.Position);
      }
      else
      {
        SetPositionTarget(f, character, position, target);
      }
    }

    public static void SetPositionTarget(Frame f, EntityRef character, FPVector2 position, FPVector2 target)
    {
      var distance = 1;
      if (FPVector2.DistanceSquared(position, target) > distance * distance)
      {
        var direction = target - position;
        f.Signals.OnCharacterMove(character, direction);
      }
      else
      {
        f.Signals.OnCharacterMove(character, FPVector2.Zero);
      }
    }
  }
}
