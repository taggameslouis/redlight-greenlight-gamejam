namespace Quantum
{
    unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
    {
        public void OnPlayerDataSet(Frame frame, PlayerRef player)
        {
            var data = frame.GetPlayerData(player);

            var prototype = frame.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);
            var entity = frame.Create(prototype);
            
            // Add flag component so systems can run on players who aren't 'safe'
            frame.Add<ActivePlayer>(entity);
                        
            var gameSpec = frame.FindAsset<GameSpec>(frame.RuntimeConfig.GameSpec.Id);
            var startPosition = gameSpec.FindSpawnPositionForPlayer(player);

            if (frame.Unsafe.TryGetPointer<CharacterFields>(entity, out var characterFields))
            {
                characterFields->Player = player;
                characterFields->InitialPosition = startPosition;
            }

            if (frame.Unsafe.TryGetPointer<Transform2D>(entity, out var transform))
            {
                transform->Position = startPosition;
            }

            Log.Debug($"[PlayerSpawnSystem] OnPlayerDataSet has spawned player {player}");
        }
    }
}