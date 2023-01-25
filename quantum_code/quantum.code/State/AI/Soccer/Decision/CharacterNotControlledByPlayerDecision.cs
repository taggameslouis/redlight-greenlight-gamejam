
using System;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  public unsafe class CharacterNotControlledByPlayerDecision : HFSMDecision
  {
    public override bool Decide(Frame f, EntityRef e)
    {
      var players = f.Global->Players;
      if (e == players[0].ControlledCharacter || e == players[1].ControlledCharacter)
      {
        return false;
      }
      return true;
    }
  }
}
