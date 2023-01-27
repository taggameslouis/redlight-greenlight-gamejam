using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
    public unsafe class KillSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            if (f.Global->CurrentLightState != TrafficLightState.Red)
                return;

            
            foreach (var (entity, character) in f.Unsafe.GetComponentBlockIterator<CharacterFields>())
            {
                if (character->State != CharacterState.ACTIVE)
                    continue;

                var input = f.GetPlayerInput(character->Player);
                if (input->Direction != FPVector2.Zero)
                {
                    character->State = CharacterState.DEAD;
                    character->RespawnTimer = f.Global->RespawnDuration;

                    f.Events.CharacterStateChanged(entity, CharacterState.DEAD);
                }
            }
        }
    }
}