
using System;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  public unsafe class BallOwnerIsTeammateDecision : HFSMDecision
  {
    public override bool Decide(Frame f, EntityRef e)
    {
      return false;
      // var fields = f.Get<CharacterFields>(e);
      // var ballOwnerFields = f.Get<CharacterFields>(f.Global->BallOwner);
      // return ballOwnerFields.Player == fields.Player;
    }
  }
}
