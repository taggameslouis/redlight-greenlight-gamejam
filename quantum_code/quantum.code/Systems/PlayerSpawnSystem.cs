namespace Quantum
{
    unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
    {
        public void OnPlayerDataSet(Frame frame, PlayerRef player)
        {
            Log.Warn("OnPlayerDataSet");
            
            var data = frame.GetPlayerData(player);

            // resolve the reference to the prototpye.
            var prototype = frame.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);

            // Create a new entity for the player based on the prototype.
            var entity = frame.Create(prototype);

            // Create a PlayerLink component. Initialize it with the player. Add the component to the player entity.
            var characterFields = frame.Get<CharacterFields>(entity);
            characterFields.Player = player;
            frame.Add(entity, characterFields);

            // Offset the instantiated object in the world, based in its ID.
            if (frame.Unsafe.TryGetPointer<Transform2D>(entity, out var transform))
            {
                var gameSpec = frame.FindAsset<GameSpec>(frame.RuntimeConfig.GameSpec.Id);
                transform->Position = gameSpec.FindSpawnPositionForPlayer(player);
                Log.Warn($"Spawned at {transform->Position}");
            }
        }
    }
}