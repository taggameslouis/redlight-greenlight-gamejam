using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe class BallHasOwnerDecision : HFSMDecision
    {
        public override bool Decide(Frame f, EntityRef e)
        {
            return false;
            //return f.Exists(f.Global->BallOwner);
        }
    }
}