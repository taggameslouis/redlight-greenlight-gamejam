using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum
{
    partial class RuntimePlayer
    {
        public string Nickname;
        public AssetRefEntityPrototype CharacterPrototype;

        partial void SerializeUserData(BitStream stream)
        {
            stream.Serialize(ref Nickname);
            stream.Serialize(ref CharacterPrototype);
        }
    }
}