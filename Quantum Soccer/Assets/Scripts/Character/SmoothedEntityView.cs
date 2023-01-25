using System;
using UnityEngine;
using Quantum;

public class SmoothedEntityView : EntityView
{

  [Header("Smoothing Parameters")]
  [Range(0, 1)] public Single InterpolatePositionFactor = 0.05f;
  [Range(0, 1)] public Single InterpolateRotationFactor = 0.05f;

  protected override void ApplyTransform(ref UpdatePostionParameter param)
  {
    var framePredicted = QuantumRunner.Default?.Game?.Frames.Predicted;
    if (framePredicted == null) return;

    var player = framePredicted.Get<CharacterFields>(EntityRef).Player;
    var isRemote = !QuantumRunner.Default.Game.PlayerIsLocal(player);

    var correctedPos = param.NewPosition + param.ErrorVisualVector;
    var correctedRot = param.ErrorVisualQuaternion * param.NewRotation;

    transform.position = correctedPos;
    transform.rotation = isRemote ? Quaternion.Slerp(transform.rotation, param.NewRotation, Time.deltaTime / InterpolateRotationFactor) : correctedRot;
  }
}
