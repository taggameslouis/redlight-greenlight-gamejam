using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  public unsafe class CustomGravitySystem : SystemMainThread
  {
    public override void Update(Frame f)
    {
      var customGravity = f.Unsafe.GetPointer<CustomGravity>(f.Global->Ball);
      var transform = f.Unsafe.GetPointer<Transform2D>(f.Global->Ball);
      var transformVertical = f.Unsafe.GetPointer<Transform2DVertical>(f.Global->Ball);
      var gamespec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);

      if (customGravity->VerticalSpeed <= 0)
      {
        var position = new FPVector3(transform->Position.X, transformVertical->Position, transform->Position.Y);
        var distance = transformVertical->Height + FPMath.Abs(customGravity->VerticalSpeed * f.DeltaTime);
        var groundPosition = new FPVector3(position.X, 0, position.Z);
        FP groundHeight = 0;
        if (FPVector3.Distance(position, groundPosition) <= distance - (gamespec.BallRadius + FP._0_20))
        {
          if (customGravity->VerticalSpeed > gamespec.BallBounceThreshold)
          {
            transformVertical->Position = groundHeight;
            customGravity->Grounded = true;
          }
          else
          {
            if (customGravity->VerticalSpeed < 2 * gamespec.BallBounceThreshold)
            {
              f.Events.BallBounce();
            }
            customGravity->VerticalSpeed = -customGravity->VerticalSpeed / (1 + gamespec.BallRestitution);
          }
        }
        else
        {
          customGravity->Grounded = false;
        }
      }
      else
      {
        customGravity->Grounded = false;
      }
      if (customGravity->Grounded == false)
      {
        transformVertical->Position += (customGravity->VerticalSpeed) * f.DeltaTime;
        customGravity->VerticalSpeed += gamespec.BallGravityForce * f.DeltaTime;
      }
    }
  }
}