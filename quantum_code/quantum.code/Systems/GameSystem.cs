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
			f.Global->RespawnDuration = gameSpec.RespawnDuration;
			
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
						var playerCount = f.ComponentCount<CharacterFields>();
						if (playerCount >= f.Global->ConnectionCount)
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
						f.Events.OnTrafficLightStateChanged();
						
						Log.Debug("[GameSystem] State has been changed to RUNNING");
					}

					break;
				}

				case GameState.Running:
				{
					f.Global->MatchTimer -= f.DeltaTime;
					var gameOver = false;

					if (f.Global->MatchTimer <= FP._0)
					{
						// Timer expired!
						gameOver = true;
					}
					else
					{
						gameOver = true;
						
						// Allow early finish if all done
						foreach (var (entity, character) in f.Unsafe.GetComponentBlockIterator<CharacterFields>())
						{
							if (character->State == CharacterState.ACTIVE ||
							    character->State == CharacterState.DEAD)
							{
								gameOver = false;
								break;
							}
						}
					}

					if (gameOver)
					{
						f.Global->CurrentGameState = GameState.Ended;
						f.Events.OnGameStateChanged(f.Global->CurrentGameState);

						foreach (var (entity, character) in f.Unsafe.GetComponentBlockIterator<CharacterFields>())
						{
							var isEliminated = character->State == CharacterState.ACTIVE;
							f.Events.OnGameEnded(character->Player, isEliminated);
						}

						Log.Debug("[GameSystem] State has been changed to ENDED");
					}

					break;
				}

				case GameState.Ended:
				{
					break;
				}
			}
		}
	}
}