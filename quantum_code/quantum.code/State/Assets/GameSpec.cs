using Photon.Deterministic;

namespace Quantum
{
    public partial class GameSpec
    {
        public FP WaitingForConnectionsTimer = FP._5;
        public FP InitialCountdown = FP._10;
        public FP MatchDuration = 120;
        public FP RespawnDuration = FP._2;
        
        public FP RedLightMinDuration;
        public FP RedLightMaxDuration;
        public FP AmberLightDuration;
        public FP GreenLightMinDuration;
        public FP GreenLightMaxDuration;

        public FPVector2 DefaultSpawnPosition = FPVector2.Zero;
        public FPVector2 DefaultSpawnPositionSpacing = FPVector2.One * FP._10;
        public int MaxNumberOfColumns = 10;

        public FPVector2 FindSpawnPositionForPlayer(int playerId)
        {
            var x = playerId % MaxNumberOfColumns;
            var y = playerId / MaxNumberOfColumns;

            var position = DefaultSpawnPosition;
            position.X -= (MaxNumberOfColumns * FP._0_50) * DefaultSpawnPositionSpacing.X;

            position.X += DefaultSpawnPositionSpacing.X * x;
            position.Y -= DefaultSpawnPositionSpacing.Y * y;

            return position;
        }
    }
}