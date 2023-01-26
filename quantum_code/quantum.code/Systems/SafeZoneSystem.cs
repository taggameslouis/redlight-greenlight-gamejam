using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
	public class SafeZoneSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D
	{
		public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
		{
			if (f.Has<ActivePlayer>(info.Entity))
			{
				f.Remove<ActivePlayer>(info.Entity);
				
				f.Signals.OnGoal();
				f.Events.OnGoal();
				
				Log.Debug("[SafeZoneSystem] Player has reached the safe zone");
			}
		}
	}
}
