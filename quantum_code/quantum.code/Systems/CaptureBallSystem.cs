using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  public unsafe class CaptureBallSystem : SystemMainThread, ISignalOnCollisionEnter2D
  {
    public override void OnInit(Frame f)
    {
      foreach (var (ball, fields) in f.Unsafe.GetComponentBlockIterator<BallFields>())
      {
        f.Global->Ball = ball;
      }
    }
    public void OnCollisionEnter2D(Frame f, CollisionInfo2D info)
    {
      // TODO refactor
      if (info.Other == EntityRef.None)
      {
        f.Events.BallCollide();
        return;
      }

      f.Unsafe.TryGetPointer<BallFields>(info.Entity, out var ballFields);
      f.Unsafe.TryGetPointer<BallFields>(info.Other, out var otherBallFields);
      if (ballFields == null && otherBallFields == null)
      {
        return;
      }
      var currentBallOwner = f.Global->BallOwner;
      if (ballFields == null)
      {
        f.Unsafe.TryGetPointer<CharacterFields>(info.Entity, out var characterFields);
        if (characterFields->FallenTimer > 0)
        {
          return;
        }
        f.Global->BallOwner = info.Entity;
        f.Events.CharacterCaptureBall(info.Entity);
        SetCharacterFallen(f, currentBallOwner, info.Entity);
        f.Global->Players.GetPointer(characterFields->Player)->ControlledCharacter = info.Entity;

      }
      else
      {
        f.Unsafe.TryGetPointer<CharacterFields>(info.Other, out var characterFields);
        if (characterFields->FallenTimer > 0)
        {
          return;
        }
        f.Global->BallOwner = info.Other;
        f.Events.CharacterCaptureBall(info.Other);
        SetCharacterFallen(f, currentBallOwner, info.Other);
        f.Global->Players.GetPointer(characterFields->Player)->ControlledCharacter = info.Entity;
      }
    }

    private void SetCharacterFallen(Frame f, EntityRef character, EntityRef newBallOwner)
    {
      if (f.Exists(character) == false)
      {
        return;
      }
      if (f.TryGet<CharacterFields>(character, out var characterFields) && f.TryGet<CharacterFields>(newBallOwner, out var newBallOwnerFields))
      {
        if (characterFields.Player == newBallOwnerFields.Player)
        {
          return;
        }
        f.Signals.OnCharacterFall(character);
      }
    }

    public override void Update(Frame f)
    {
      if (f.Exists(f.Global->BallOwner) && f.Global->Ball != EntityRef.None)
      {
        var transform = f.Unsafe.GetPointer<Transform2D>(f.Global->Ball);
        var ballFields = f.Unsafe.GetPointer<BallFields>(f.Global->Ball);
        var ballBody = f.Unsafe.GetPointer<PhysicsBody2D>(f.Global->Ball);

        ballBody->Velocity = FPVector2.Zero;

        var characterTransform = f.Unsafe.GetPointer<Transform2D>(f.Global->BallOwner);
        var kccSpeed = f.Unsafe.GetPointer<KCC>(f.Global->BallOwner)->Velocity.Magnitude;
        var targetPosition = characterTransform->Position + characterTransform->Right;
        var lerpSpeed = (kccSpeed + 3);
        lerpSpeed = FPMath.Clamp(lerpSpeed, 12, 17) * f.DeltaTime;
        var distance = FP._0_20;
        if (FPVector2.DistanceSquared(transform->Position, targetPosition) > distance * distance)
        {
          transform->Position = FPVector2.Lerp(transform->Position, targetPosition, lerpSpeed);
        }
      }
    }
  }
}