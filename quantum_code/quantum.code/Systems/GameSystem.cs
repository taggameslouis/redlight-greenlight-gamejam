using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
	public unsafe class GameSystem : SystemMainThread, ISignalOnGoal, ISignalOnMatchEnd
	{
		public void OnGoal(Frame f)
		{
			var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
			f.Global->GoalDelayTimer = gameSpec.OnGoalDelay;
			f.Global->State = GameState.GoalDelay;
			f.Global->IsFirstMatch = true;
		}

		public override void OnInit(Frame f)
		{
			f.Global->IsFirstMatch = true;
			SetMatchTime(f);
			SetInitialKickDelay(f);
		}

		private void SetMatchTime(Frame f)
		{
			var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
			f.Global->MatchTimer = gameSpec.MatchDuration;
		}

		private void SetInitialKickDelay(Frame f)
		{
			f.Global->State = GameState.InitialKickDelay;
			var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
			f.Global->InitialKickDelay = gameSpec.InitialKickDelay;
		}

		public void OnInitialKick(Frame f)
		{
		}

		public override void Update(Frame f)
		{
			if (f.Global->State == GameState.InitialKickDelay)
			{
				if (f.Global->InitialKickDelay > 0)
				{
					f.Global->InitialKickDelay -= f.DeltaTime;
				}
				else
				{
					f.Global->State = GameState.Running;
				}
			}

			if (f.Global->MatchTimer > 0)
			{
				f.Global->MatchTimer -= f.DeltaTime;
			}
			else
			{
				if (f.Global->State != GameState.Ended)
				{
					f.Signals.OnMatchEnd();
				}
			}

			if (f.Global->State == GameState.GoalDelay)
			{
				if (f.Global->GoalDelayTimer > 0)
				{
					f.Global->GoalDelayTimer -= f.DeltaTime;
				}
				else
				{
					f.Signals.OnGoalDelayEnd();
					SetInitialKickDelay(f);
				}
			}
		}

		public void OnMatchEnd(Frame f)
		{
			f.Events.OnGameEnd();
			if (f.Global->IsFirstMatch)
			{
				f.Global->State = GameState.Paused;
				StartSecondMatch(f);
			}
			else
			{
				f.Global->State = GameState.Ended;
			}
		}

		private void StartSecondMatch(Frame f)
		{
			f.Global->IsFirstMatch = false;
			f.Global->State = GameState.InitialKickDelay;

			f.Global->LastGoalTeam = 0;
			var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
			f.Global->MatchTimer = gameSpec.MatchDuration;
		}
	}
}