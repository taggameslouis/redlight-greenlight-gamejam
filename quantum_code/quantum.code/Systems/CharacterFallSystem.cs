using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  public unsafe class CharacterFallSystem : SystemMainThread, ISignalOnCharacterFall
  {
    public void OnCharacterFall(Frame f, EntityRef character)
    {
      var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
      var characterFields = f.Unsafe.GetPointer<CharacterFields>(character);
      characterFields->FallenTimer = gameSpec.FallenTime;
      f.Signals.OnSwitchCharacter(characterFields->Player);
      f.Events.CharacterFall(character);
    }

    public override void Update(Frame f)
    {
      foreach (var (character, fields) in f.Unsafe.GetComponentBlockIterator<CharacterFields>())
      {
        if (fields->FallenTimer > 0)
        {
          fields->FallenTimer -= f.DeltaTime;
        }
        else {
          fields->FallenTimer = 0;
        }
      }
    }
  }
}
