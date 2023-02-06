namespace Quantum
{
    unsafe class PlayerSpawnSystem : SystemMainThread, ISignalOnPlayerDataSet
    {
        public override void Update(Frame f)
        {
            if (f.Global->CurrentGameState != GameState.Running)
                return;
				
            foreach (var (entity, character) in f.Unsafe.GetComponentBlockIterator<CharacterFields>())
            {
                if (character->State != CharacterState.DEAD)
                    continue;

                character->RespawnTimer -= f.DeltaTime;
                if (character->RespawnTimer < Photon.Deterministic.FP._0)
                {
                    character->State = CharacterState.ACTIVE;

                    var transform = f.Unsafe.GetPointer<Transform2D>(entity);
                    transform->Position = character->InitialPosition;
                    
                    f.Events.CharacterStateChanged(entity, CharacterState.ACTIVE);
                }
            }
        }

        public void OnPlayerDataSet(Frame frame, PlayerRef player)
        {
            var data = frame.GetPlayerData(player);
            
            Log.Debug($"Retrieving player data for player {player} with nickname {data.Nickname}");

            var prototype = frame.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);
            var entity = frame.Create(prototype);
                        
            var gameSpec = frame.FindAsset<GameSpec>(frame.RuntimeConfig.GameSpec.Id);
            var startPosition = gameSpec.FindSpawnPositionForPlayer(player);

            if (frame.Unsafe.TryGetPointer<CharacterFields>(entity, out var characterFields))
            {
                characterFields->Player = player;
                characterFields->InitialPosition = startPosition;
                characterFields->Nickname = data.Nickname;
                characterFields->State = CharacterState.ACTIVE;
                characterFields->RespawnTimer = Photon.Deterministic.FP._0;
            }

            if (frame.Unsafe.TryGetPointer<Transform2D>(entity, out var transform))
            {
                transform->Position = startPosition;
                transform->Rotation = 90 * Photon.Deterministic.FP.Deg2Rad;
            }

            Log.Debug($"[PlayerSpawnSystem] OnPlayerDataSet has spawned player {player}");
        }
    }
}