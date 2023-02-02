using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
    public unsafe class TrafficLightSystem : SystemMainThread
    {
        public override void OnInit(Frame f)
        {
            var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
            
            f.Global->RedLightMinDuration = gameSpec.RedLightMinDuration;
            f.Global->RedLightMaxDuration = gameSpec.RedLightMaxDuration;
            f.Global->AmberLightDuration = gameSpec.AmberLightDuration;
            f.Global->GreenLightMinDuration = gameSpec.GreenLightMinDuration;
            f.Global->GreenLightMaxDuration = gameSpec.GreenLightMaxDuration;

            f.Global->CurrentLightState = TrafficLightState.Green;
            f.Global->CurrentLightDuration = GetRandomTime(f);
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
                    f.Global->CurrentLightState = TrafficLightState.Red;
                    f.Global->CurrentLightDuration = GetRandomTime(f);
                    break;
                }

                case TrafficLightState.Red:
                {
                    f.Global->CurrentLightState = TrafficLightState.Green;
                    f.Global->CurrentLightDuration = GetRandomTime(f);
                    break;
                }

                case TrafficLightState.Green:
                {
                    f.Global->CurrentLightState = TrafficLightState.Amber;
                    f.Global->CurrentLightDuration = f.Global->AmberLightDuration;
                    break;
                }
            }

            f.Events.OnTrafficLightStateChanged();
            f.Signals.OnTrafficLightStateChanged();
            
            Log.Debug($"[TrafficLightSystem] Light changed to {f.Global->CurrentLightState}");
        }

        private FP GetRandomTime(Frame f)
        {
            switch (f.Global->CurrentLightState)
            {
                case TrafficLightState.Green:
                    return f.RNG->Next(f.Global->GreenLightMinDuration, f.Global->GreenLightMaxDuration);
                
                case TrafficLightState.Red:
                    return f.RNG->Next(f.Global->RedLightMinDuration, f.Global->RedLightMaxDuration);
            }

            return FP._0;
        }
    }
}