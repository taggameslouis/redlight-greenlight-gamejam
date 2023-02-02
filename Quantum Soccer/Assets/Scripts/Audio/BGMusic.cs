using UnityEngine;
using Quantum;

public unsafe class BGMusic : MonoBehaviour
{
    private void Awake()
    {
        QuantumEvent.Subscribe<EventOnTrafficLightStateChanged>(this, OnLightStateChanged);
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }

    private void OnLightStateChanged(EventOnTrafficLightStateChanged callback)
    {
        switch (callback.Game.Frames.Verified.Global->CurrentLightState)
        {
            case TrafficLightState.Amber:
            {
                AudioManager.Instance.StopBGMusic();
                break;
            }
            case TrafficLightState.Green:
            {
                AudioManager.Instance.PlayBGMusic("intense_bg");
                break;
            }
        }
    }
}