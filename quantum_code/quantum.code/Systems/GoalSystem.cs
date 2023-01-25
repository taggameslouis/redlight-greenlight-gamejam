using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
	public unsafe class GoalSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D
	{
		public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
		{
			if (f.Global->GoalDelayTimer > 0) {
				return;
			}
			if (f.TryGet<BallFields>(info.Entity, out var ball))
			{
				var position = f.Get<Transform2D>(info.Entity).Position;
				/*if (position.X > 0)
				{
					f.Global->Players.GetPointer(0)->PlayerScore++;
					f.Global->LastGoalTeam = 0;
				}
				else
				{
					f.Global->Players.GetPointer(1)->PlayerScore++;
					f.Global->LastGoalTeam = 1;
				}*/
			}
			f.Signals.OnGoal();
			f.Events.OnGoal();
		}
	}
}
