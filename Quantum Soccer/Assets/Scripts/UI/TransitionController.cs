using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;


public class TransitionController : MonoBehaviour
{
    public GameObject Transition;

    private void Start()
    {
        QuantumEvent.Subscribe<EventOnGameEnd>(this, OnGameEnd);
    }

    private void OnGameEnd(EventOnGameEnd e)
    {
        Transition.SetActive(true);
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}