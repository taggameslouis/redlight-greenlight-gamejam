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
    
    public Image LightImg;

    private void Awake()
    {
        QuantumEvent.Subscribe<EventOnTrafficLightStateChanged>(this, OnTrafficLightStateChanged);
        QuantumEvent.Subscribe<EventOnGameStateChanged>(this, OnGameStateChanged);

        LightImg.color = DefaultColor;
    }

    private void OnGameStateChanged(EventOnGameStateChanged callback)
    {
        if (callback.NewGameState == GameState.Starting)
        {
            OnTrafficLightStateChanged(null);
        }
    }

    private void OnTrafficLightStateChanged(EventOnTrafficLightStateChanged callback)
    {
        switch (QuantumRunner.Default.Game.Frames.Verified.Global->CurrentLightState)
        {
            case TrafficLightState.Green:
            {
                LightImg.color = GreenColor;
                break;
            }

            case TrafficLightState.Amber:
            case TrafficLightState.Red:
            {
                LightImg.color = RedColor;
                break;
            }
        }
    }
}