using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  public unsafe class CharacterSlideSystem : SystemMainThread
  {
    public override void Update(Frame f)
    {
      if (f.Global->State != GameState.InitialKick && f.Global->State != GameState.Running)
      {
        //return;
      }

      for (int i = 0; i < f.Global->Players.Length; i++)
      {
        var player = f.Global->Players.GetPointer(i);
        var character = player->ControlledCharacter;
        var transform = f.Get<Transform2D>(character);
        var fields = f.Unsafe.GetPointer<CharacterFields>(character);
        var kcc = f.Unsafe.GetPointer<KCC>(character);

        var input = f.GetPlayerInput(fields->Player);

        fields->SlideTimer -= f.DeltaTime;
        if (fields->SlideTimer >= 0)
        {
          fields->IsSliding = false;
        }

        var spec = f.FindAsset<CharacterSpec>(fields->Spec.Id);

        if (input->Slide.IsDown &&
          fields->SlideTimer <= -1 &&
          (f.Global->Players[0].ControlledCharacter == character || f.Global->Players[1].ControlledCharacter == character) &&
          f.Global->BallOwner != character)
        {

          fields->SlideTimer = spec.SlideDuration;
          fields->IsSliding = true;
          f.Events.CharacterSlide(character);

          if (input->Direction == FPVector2.Zero)
          {
            fields->SlideDirection = transform.Forward;
          }
          else
          {
            fields->SlideDirection = input->Direction;
          }
        }

        if (fields->IsSliding)
        {
          PerformSlide(f, character, fields, spec.SlideSettings, kcc);
        }
      }
    }

    private void PerformSlide(Frame f, EntityRef character, CharacterFields* fields, AssetRefKCCSettings settingsRef, KCC* kcc)
    {
      var settings = f.FindAsset<KCCSettings>(settingsRef.Id);
      settings.Init(ref *kcc);
      var movement = settings.ComputeRawMovement(f, character, fields->SlideDirection);
      settings.SteerAndMove(f, character, in movement);
    }
  }
}