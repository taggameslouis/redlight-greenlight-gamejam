using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;


public class TransitionController : MonoBehaviour
{
    public GameObject Transition;

    private void Start()
    {
        QuantumEvent.Subscribe<EventOnGameStateChanged>(this, OnGameEnd);
    }

    private void OnGameEnd(EventOnGameStateChanged e)
    {
        if (e.NewGameState == GameState.Ended)
        {
            Transition.SetActive(true);
        }
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}