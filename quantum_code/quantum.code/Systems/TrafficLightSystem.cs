using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
    public unsafe class TrafficLightSystem : SystemMainThread
    {
        public override void OnInit(Frame f)
        {
            var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
            
            f.Global->RedLightDuration = gameSpec.RedLightDuration;
            f.Global->AmberLightDuration = gameSpec.AmberLightDuration;
            f.Global->GreenLightDuration = gameSpec.GreenLightDuration;

            f.Global->CurrentLightState = TrafficLightState.Green;
            f.Global->CurrentLightDuration = f.Global->GreenLightDuration;
            f.Events.TrafficLightStateChanged();
        }

        public override void Update(Frame f)
        {
            if (f.Global->CurrentGameState != GameState.Running)
                return;

            f.Global->CurrentLightDuration -= f.DeltaTime;
            
            if (f.Global->CurrentLightDuration > FP._0)
                return;
            
            switch (f.Global->CurrentLightState)
            {
                case TrafficLightState.Amber:
                {
                    f.Global->CurrentLightDuration = f.Global->RedLightDuration;
                    f.Global->CurrentLightState = TrafficLightState.Red;
                    break;
                }

                case TrafficLightState.Red:
                {
                    f.Global->CurrentLightDuration = f.Global->GreenLightDuration;
                    f.Global->CurrentLightState = TrafficLightState.Green;
                    break;
                }

                case TrafficLightState.Green:
                {
                    f.Global->CurrentLightDuration = f.Global->AmberLightDuration;
                    f.Global->CurrentLightState = TrafficLightState.Amber;
                    break;
                }
            }

            f.Events.TrafficLightStateChanged();
            
            Log.Debug($"[TrafficLightSystem] Light changed to {f.Global->CurrentLightState}");
        }
    }
}