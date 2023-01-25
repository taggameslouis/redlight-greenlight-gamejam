using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  public unsafe class RespawnSystem : SystemSignalsOnly, ISignalOnGoalDelayEnd, ISignalOnMatchEnd
  {
    public override void OnInit(Frame f)
    {
      var charactersFilter = f.Filter<Transform2D, CharacterFields>();
      while (charactersFilter.NextUnsafe(out var character, out var transform, out var fields))
      {
        fields->InitialPosition = transform->Position;
      }
      f.Global->LastGoalTeam = 1;
      SetStarterCharacter(f);
    }

    public void OnGoalDelayEnd(Frame f)
    {
      ResetField(f);
    }

    private void ResetField(Frame f)
    {
      var ballTransform = f.Unsafe.GetPointer<Transform2D>(f.Global->Ball);
      ballTransform->Position = FPVector2.Zero;
      var ballBody = f.Unsafe.GetPointer<PhysicsBody2D>(f.Global->Ball);
      ballBody->Velocity = FPVector2.Zero;

      var charactersFilter = f.Filter<Transform2D, CharacterFields>();
      while (charactersFilter.NextUnsafe(out var character, out var transform, out var fields))
      {
        transform->Position = fields->InitialPosition;
        if (fields->Player == 1)
        {
          transform->Rotation = 90;
        }
        else
        {
          transform->Rotation = -90;
        }
      }
      SetStarterCharacter(f);
    }

    private void SetStarterCharacter(Frame f)
    {
      var charactersFilter = f.Filter<Transform2D, CharacterFields>();
      while (charactersFilter.NextUnsafe(out var character, out var transform, out var fields))
      {
        if (fields->Player == f.Global->LastGoalTeam)
        {
          continue;
        }
        if (fields->InitWithBall)
        {
          transform->Position = FPVector2.Zero;
          f.Global->BallOwner = character;
        }
      }
    }

    public void OnMatchEnd(Frame f)
    {
      if (f.Global->State == GameState.Paused || f.Global->State  == GameState.InitialKickDelay)
      {
        ResetField(f);
      }
    }
  }
}
