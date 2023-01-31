using UnityEngine;
using Quantum;

public unsafe class BGMusic : MonoBehaviour
{
    private void Awake()
    {
        QuantumEvent.Subscribe<EventTrafficLightStateChanged>(this, OnLightStateChanged);
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }

    private void OnLightStateChanged(EventTrafficLightStateChanged callback)
    {
        switch (callback.Game.Frames.Verified.Global->CurrentLightState)
        {
            case TrafficLightState.Amber:
            {
                AudioManager.Instance.Stop();
                break;
            }
            case TrafficLightState.Green:
            {
                AudioManager.Instance.Play("intense_bg");
                break;
            }
        }
    }
}