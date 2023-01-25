using System;
using UnityEngine;
using Quantum;

public unsafe class SmoothedBallEntityView : EntityView
{
    [Header("Smoothing Parameters")] [Range(0, 1)]
    public float InterpolatePositionFactor = 0.05f;

    [Range(0, 1)] public float InterpolateRotationFactor = 0.05f;

    protected override void ApplyTransform(ref UpdatePostionParameter param)
    {
        var framePredicted = QuantumRunner.Default?.Game?.Frames.Predicted;
        if (framePredicted == null) return;

        var isRemote = false;
        if (framePredicted.TryGet<CharacterFields>(framePredicted.Global->BallOwner, out var characterFields))
            isRemote = !QuantumRunner.Default.Game.PlayerIsLocal(characterFields.Player);

        var correctedPos = param.NewPosition + param.ErrorVisualVector;
        var correctedRot = param.ErrorVisualQuaternion * param.NewRotation;

        transform.position = correctedPos;
        transform.rotation = isRemote
            ? Quaternion.Slerp(transform.rotation, param.NewRotation, Time.deltaTime / InterpolateRotationFactor)
            : correctedRot;
    }
}