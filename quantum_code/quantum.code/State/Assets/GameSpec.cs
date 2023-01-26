using Photon.Deterministic;

namespace Quantum
{
    public partial class GameSpec
    {
        public FP InitialCountdown = 10;
        public FP MatchDuration = 120;
        
        public FP RedLightDuration;
        public FP AmberLightDuration;
        public FP GreenLightDuration;

        public FPVector3 DefaultSpawnPosition = FPVector3.Zero;
        public FPVector2 DefaultSpawnPositionSpacing = FPVector2.One * FP._10;
        public int MaxNumberOfColumns = 10;

        public FP MaxCharacterPositionX = 41;
        public FP MaxCharacterPositionY = 31;

        public FPVector2 FindSpawnPositionForPlayer(int playerId)
        {
            var x = playerId % MaxNumberOfColumns;
            var y = playerId / MaxNumberOfColumns;

            var position = DefaultSpawnPosition;
            position.X -= (MaxNumberOfColumns * FP._0_50) * DefaultSpawnPositionSpacing.X;

            position.X += DefaultSpawnPositionSpacing.X * x;
            position.Y -= DefaultSpawnPositionSpacing.Y * y;

            return position.XY;
        }
    }
}