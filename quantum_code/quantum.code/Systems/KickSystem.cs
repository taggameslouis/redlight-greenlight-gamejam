using System;
using Photon.Deterministic;

namespace Quantum
{
  unsafe class KickSystem : SystemMainThread
  {
    public override void Update(Frame f)
    {
      if (f.Global->State != GameState.InitialKick && f.Global->State != GameState.Running)
      {
        return;
      }
      if (f.Exists(f.Global->BallOwner) == false)
      {
        return;
      }

      var t = f.Unsafe.GetPointer<Transform2D>(f.Global->Ball);
      t->Rotation = 0;

      var fields = f.Get<CharacterFields>(f.Global->BallOwner);
      var player = f.Global->Players.GetPointer(fields.Player);

      var input = f.GetPlayerInput(fields.Player);
      if (input->Kick.IsDown)
      {
        player->HoldKickTimer += f.DeltaTime;
      }

      if (player->HoldKickTimer > 1 || input->Kick.WasReleased) {
        PerformKick(f, fields, player->HoldKickTimer);
        player->HoldKickTimer = FP._0_33;
      }
    }

    private void PerformKick(Frame f, CharacterFields fields, FP force) {
      var transform = f.Get<Transform2D>(f.Global->BallOwner);
      var kcc = f.Get<KCC>(f.Global->BallOwner);

      if (f.Global->State == GameState.InitialKick)
      {
        f.Signals.OnInitialKick();
      }
      var ball = f.Global->Ball;

      if (kcc.Velocity.Magnitude != 0)
      {
        var ballTransform = f.Unsafe.GetPointer<Transform2D>(ball);
        ballTransform->Position = transform.Position + transform.Right;
      }

      var spec = f.FindAsset<CharacterSpec>(fields.Spec.Id);

      var ballBody = f.Unsafe.GetPointer<PhysicsBody2D>(ball);
      ballBody->AddForce(transform.Forward * spec.KickForce * force);
      var ballFields = f.Unsafe.GetPointer<BallFields>(ball);
      var ballGravity = f.Unsafe.GetPointer<CustomGravity>(ball);
      ballGravity->VerticalSpeed = spec.KickHeight * force;
      f.Events.CharacterKick(f.Global->BallOwner);
      f.Global->BallOwner = EntityRef.None;
    }
  }
}
