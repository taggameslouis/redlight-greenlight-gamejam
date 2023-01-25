using Photon.Deterministic;

namespace Quantum
{
  public partial class CharacterSpec
  {
    public AssetRefKCCSettings DefaultRunSettings;
    public AssetRefKCCSettings SprintRunSettings;
    public AssetRefKCCSettings SlideSettings;

    public FP SlideDuration;
    public FP StaminaUsageVelocity;
    public FP StaminaRegenerationVelocity;

    public FP PassAngle = 45;
    public FP PassForce = 1200;
    public FP PassHeightFactor = 5;

    public FP KickForce = 1200;
    public FP KickHeight = 10;
  }
}
