using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
	public unsafe class SafeZoneSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D
	{
		public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
		{
			if (f.Unsafe.TryGetPointer<CharacterFields>(info.Entity, out var characterFields))
			{
				characterFields->State = CharacterState.INACTIVE;

				f.Events.CharacterStateChanged(info.Entity, characterFields->State);
				f.Events.OnPlayerSafe(characterFields->Player);

				Log.Debug("[SafeZoneSystem] Player has reached the safe zone");
			}
		}
	}
}
