using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;
using Photon.Deterministic;

public unsafe class BallView : MonoBehaviour
{
  public EntityView EntityView;
  public GameObject Model;
  public float Speed = 10;

  public float AnimationAmplitude = .5f;
  public float AnimationSpeed = 1;

  private bool _isForward = false;

  private void FixedUpdate()
  {

    if (EntityView.EntityRef == EntityRef.None)
    {
      return;
    }
    var f = QuantumRunner.Default.Game.Frames.Predicted;

    if (f.Global->Ball != EntityRef.None) {
      ModelAnimation();
    }

    var direction = Vector3.zero;
    float velocity = 0;
    if (f.Global->BallOwner != EntityRef.None)
    {
      var kcc = f.Get<KCC>(f.Global->BallOwner);
      velocity = (float)kcc.Velocity.Magnitude;
      direction = kcc.Velocity.XOY.ToUnityVector3();
    }
    else
    {
      var body = f.Get<PhysicsBody2D>(EntityView.EntityRef);
      velocity = (float)body.Velocity.Magnitude;
      direction = body.Velocity.XOY.ToUnityVector3();
    }
    SetModelRotation(velocity, direction);
  }

  private void ModelAnimation() {

  }

  private void SetModelRotation(float velocity, Vector3 direction)
  {
    if (velocity != 0)
    {
      var vectorRight = new Vector3(direction.z, direction.y, -direction.x);
      Model.transform.Rotate(vectorRight, velocity * Speed * Time.deltaTime, Space.World);
    }
  }
}
