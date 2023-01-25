using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  unsafe class SwitchCharacterSystem : SystemMainThread, ISignalOnSwitchCharacter, ISignalOnSwitchToCharacter
  {
    public override void OnInit(Frame f)
    {
      for (int i = 0; i < f.Global->Players.Length; i++)
      {
        f.Signals.OnSwitchCharacter(i);
      }
    }

    public void OnSwitchCharacter(Frame f, PlayerRef player)
    {
      var nextCharacter = GetNextCharacter(f, player);
      if (nextCharacter != EntityRef.None)
      {
        f.Events.CharacterSwitch(player, nextCharacter);
        f.Global->Players.GetPointer(player)->ControlledCharacter = nextCharacter;
      }
    }

    public void OnSwitchToCharacter(Frame f, PlayerRef player, EntityRef character)
    {
      f.Global->Players.GetPointer(player)->ControlledCharacter = character;
    }

    public override void Update(Frame f)
    {
      if (f.Global->State != GameState.Running)
      {
        return;
      }

      for (int i = 0; i < f.Global->Players.Length; i++)
      {
        var input = f.GetPlayerInput(i);
        if (input->SwitchCharacter.WasPressed)
        {
          f.Signals.OnSwitchCharacter(i);
        }
      }
    }

    private EntityRef GetNextCharacter(Frame f, PlayerRef player)
    {
      EntityRef nextCharacter = EntityRef.None;
      if (f.TryGet<CharacterFields>(f.Global->BallOwner, out var ballOwnerFields) && ballOwnerFields.Player == player)
      {
        return nextCharacter;
      }

      var currentDistance = FP.MaxValue;
      var charactersFilter = f.Filter<Transform2D, CharacterFields, KCC>();
      while (charactersFilter.NextUnsafe(out var character, out var transform, out var fields, out var kcc))
      {
        if (fields->Player != player)
        {
          continue;
        }
        if (character == f.Global->Players.GetPointer(player)->ControlledCharacter)
        {
          continue;
        }
        if (fields->FallenTimer > 0)
        {
          continue;
        }
        if (f.Global->BallOwner == character)
        {
          continue;
        }

        var ballTransform = f.Get<Transform2D>(f.Global->Ball);
        var distance = FPVector2.Distance(transform->Position, ballTransform.Position);
        if (distance < currentDistance)
        {
          currentDistance = distance;
          nextCharacter = character;
        }
      }
      return nextCharacter;
    }
  }
}
