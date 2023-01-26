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
			f.Global->WaitingForConnectionsTimer = gameSpec.WaitingForConnectionsTimer;
			
			f.Global->CurrentGameState = GameState.Pending;
			f.Events.OnGameStateChanged(f.Global->CurrentGameState);
			
			Log.Debug("[GameSystem] State has been init to PENDING");
		}

		public override void Update(Frame f)
		{
			switch (f.Global->CurrentGameState)
			{
				case GameState.Pending:
				{
					if (f.Global->WaitingForConnectionsTimer > FP._0)
					{
						f.Global->WaitingForConnectionsTimer -= f.DeltaTime;
					}
					else
					{
						var playerCount = f.ComponentCount<ActivePlayer>();
						if (playerCount >= f.PlayerCount)
						{
							f.Global->CurrentGameState = GameState.Starting;
							f.Events.OnGameStateChanged(f.Global->CurrentGameState);

							Log.Debug("[GameSystem] State has been changed to STARTING");
						}
					}

					break;
				}

				case GameState.Starting:
				{
					if (f.Global->InitialCountdown > 0)
					{
						f.Global->InitialCountdown -= f.DeltaTime;
					}
					else
					{
						f.Global->CurrentGameState = GameState.Running;
						f.Events.OnGameStateChanged(f.Global->CurrentGameState);
						
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
						f.Events.OnGameStateChanged(f.Global->CurrentGameState);
						
						f.Signals.OnMatchEnd();
						
						Log.Debug("[GameSystem] State has been changed to ENDED");
					}

					break;
				}

				case GameState.Ended:
					break;
			}
		}
	}
}