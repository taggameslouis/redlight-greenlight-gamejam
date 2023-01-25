using Photon.Deterministic;
using System;

namespace Quantum
{
    partial class RuntimeConfig
    {
        public AssetRefGameSpec GameSpec;

        partial void SerializeUserData(BitStream stream)
        {
            stream.Serialize(ref GameSpec.Id);
        }
    }
}