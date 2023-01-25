using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  public unsafe class PassSystem : SystemMainThread
  {
    public override void Update(Frame f)
    {
      if (f.Global->BallOwner != EntityRef.None)
      {
        var fields = f.Unsafe.GetPointer<CharacterFields>(f.Global->BallOwner);

        var player = f.Global->Players.GetPointer(fields->Player);
        var input = f.GetPlayerInput(fields->Player);
        if (input->Pass.IsDown)
        {
          player->HoldPassTimer += f.DeltaTime;
        }

        if (player->HoldPassTimer > 1 || input->Pass.WasReleased)
        {
          GetPassDirection(f);
          player->HoldPassTimer = 0;
        }
      }
    }

    private void GetPassDirection(Frame f)
    {
      var characterFields = f.Unsafe.GetPointer<CharacterFields>(f.Global->BallOwner);
      var characterTransform = f.Unsafe.GetPointer<Transform2D>(f.Global->BallOwner);
      var spec = f.FindAsset<CharacterSpec>(characterFields->Spec.Id);
      var kcc = f.Unsafe.GetPointer<KCC>(f.Global->BallOwner);

      FP closestDistance = FP.MaxValue;
      FPVector2 targetPosition = FPVector2.Zero;

      var charactersFilter = f.Filter<Transform2D, CharacterFields>();
      while (charactersFilter.NextUnsafe(out var character, out var transform, out var fields))
      {
        if (fields->Player != characterFields->Player)
        {
          continue;
        }
        if (character == f.Global->BallOwner)
        {
          continue;
        }
        if (GetCharacterAngle(f, characterTransform, transform) < spec.PassAngle)
        {
          var distance = FPVector2.Distance(characterTransform->Position, transform->Position);
          if (distance < closestDistance)
          {
            targetPosition = transform->Position;
            closestDistance = distance;
          }
        }
      }
      if (closestDistance < FP.MaxValue)
      {
        PerformPass(f, characterTransform, targetPosition, kcc, spec);
      }
      else
      {
        PerformPass(f, characterTransform, characterTransform->Position + characterTransform->Forward, kcc, spec);
      }
    }

    private void PerformPass(Frame f, Transform2D* characterTranform, FPVector2 targetPosition, KCC* kcc, CharacterSpec spec)
    {
      if (f.Global->State == GameState.InitialKick)
      {
        f.Signals.OnInitialKick();
      }

      var ball = f.Global->Ball;

      if (kcc->Velocity.Magnitude != 0)
      {
        var ballTransform = f.Unsafe.GetPointer<Transform2D>(ball);
        ballTransform->Position = characterTranform->Position + characterTranform->Right;
      }

      var direction = targetPosition - characterTranform->Position;

      var ballBody = f.Unsafe.GetPointer<PhysicsBody2D>(ball);
      ballBody->AddForce(direction.Normalized * spec.PassForce);

      var ballFields = f.Unsafe.GetPointer<BallFields>(ball);
      var ballGravity = f.Unsafe.GetPointer<CustomGravity>(ball);

      var height = FPVector2.Distance(targetPosition, characterTranform->Position);
      ballGravity->VerticalSpeed = height * spec.PassHeightFactor;

      f.Events.CharacterPass(f.Global->BallOwner);
      f.Global->BallOwner = EntityRef.None;
    }

    public FP GetCharacterAngle(Frame f, Transform2D* characterTransform, Transform2D* targetTransform)
    {
      var target = targetTransform->Position - characterTransform->Position;
      return FPVector2.Angle(characterTransform->Forward, target);
    }

  }
}