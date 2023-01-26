using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
	public unsafe class GameSystem : SystemMainThread
	{
		public override void OnInit(Frame f)
		{
			var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
			f.Global->InitialCountdown = gameSpec.InitialCountdown;
			f.Global->MatchTimer = gameSpec.MatchDuration;
			f.Global->CurrentGameState = GameState.GameStart;
			
			Log.Debug("[GameSystem] Gamestate has been init to GAMESTART");
		}

		public override void Update(Frame f)
		{
			switch (f.Global->CurrentGameState)
			{
				case GameState.Paused:
					break;

				case GameState.GameStart:
				{
					if (f.Global->InitialCountdown > 0)
					{
						f.Global->InitialCountdown -= f.DeltaTime;
						Log.Debug($"[GameSystem] Initial count : {(int)f.Global->InitialCountdown}");
					}
					else
					{
						f.Global->CurrentGameState = GameState.Running;
						Log.Debug("[GameSystem] State has been changed to RUNNING");
					}

					break;
				}

				case GameState.Running:
				{
					if (f.Global->MatchTimer > 0)
					{
						f.Global->MatchTimer -= f.DeltaTime;
					}
					else
					{
						f.Global->CurrentGameState = GameState.Ended;
						Log.Debug("[GameSystem] State has been changed to ENDED");
			
						f.Events.OnGameEnd();
						f.Signals.OnMatchEnd();
					}

					break;
				}

				case GameState.Ended:
					break;
			}
		}
	}
}