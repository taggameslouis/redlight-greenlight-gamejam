using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Quantum;

public unsafe class LightBoardView : QuantumCallbacks
{
    public Color DefaultColor = Color.gray;
    public Color RedColor = Color.red;
    public Color GreenColor = Color.green;
    
    public Image RedLight;
    public Image GreenLight;

    private void Awake()
    {
        QuantumEvent.Subscribe<EventTrafficLightStateChanged>(this, OnTrafficLightStateChanged);
        QuantumEvent.Subscribe<EventOnGameStateChanged>(this, OnGameStateChanged);
    }

    private void OnGameStateChanged(EventOnGameStateChanged callback)
    {
        if (callback.NewGameState == GameState.Starting)
        {
            OnTrafficLightStateChanged(null);
        }
    }

    private void OnTrafficLightStateChanged(EventTrafficLightStateChanged callback)
    {
        switch (QuantumRunner.Default.Game.Frames.Verified.Global->CurrentLightState)
        {
            case TrafficLightState.Green:
            {
                RedLight.color = DefaultColor;
                GreenLight.color = GreenColor;
                break;
            }

            case TrafficLightState.Amber:
            case TrafficLightState.Red:
            {
                GreenLight.color = DefaultColor;
                RedLight.color = RedColor;
                break;
            }
        }
    }
}