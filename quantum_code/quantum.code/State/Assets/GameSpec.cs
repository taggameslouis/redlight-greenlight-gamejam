using Photon.Deterministic;

namespace Quantum
{
  public partial class GameSpec
  {
    public FP MatchDuration = 120;

    public FP MaxCharacterPositionX = 41;
    public FP MaxCharacterPositionY = 31;

    public FP FallenTime = 2;
    public FP InitialKickDelay = 0;
    public FP OnGoalDelay = 3;

    public FP FieldLength = 40;

    public FP BallBounceThreshold = -1;
    public FP BallRestitution = FP._0_50;
    public FP BallGravityForce = -20;

    public FP BallRadius = -FP._0_50;
  }
}
