using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject Ball;
    public float Velocity = 10;
    public float MaxX;
    public float MaxZ;
    public float MinZ;

    private Vector3 _offset;
    private Vector3 InitialRotation;
    
    private void Start()
    {
        _offset = transform.position;
    }

    private void LateUpdate()
    {
        MoveCamera(Ball.transform.position);
    }

    private void MoveCamera(Vector3 ballPosition)
    {
        if (QuantumRunner.Default == null && QuantumRunner.Default.Game == null)
            return;

        var f = QuantumRunner.Default.Game.Frames.Verified;
        if (f == null)
            return;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

        var targetPosition = Vector3.Lerp(transform.position, ballPosition + _offset, Time.deltaTime * Velocity);
        if (targetPosition.x > MaxX) targetPosition.x = MaxX;
        if (targetPosition.x < -MaxX) targetPosition.x = -MaxX;

        if (targetPosition.z > MaxZ)
            targetPosition.z = MaxZ;
        if (targetPosition.z < MinZ)
            targetPosition.z = MinZ;

        transform.position = targetPosition;
    }
}