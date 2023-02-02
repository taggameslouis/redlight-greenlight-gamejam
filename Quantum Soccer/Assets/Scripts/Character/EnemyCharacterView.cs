using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;

public unsafe class EnemyCharacterView : MonoBehaviour
{
    private static readonly Quaternion IDLE = Quaternion.Euler(0, 0, 0);
    private static readonly Quaternion RED_LIGHT = Quaternion.Euler(0, 180, 0);
    
    public float RotationSpeed = 5f;

    private Quaternion m_currentRotation;
    private Quaternion m_targetRotation;
    private float m_time = 0f;

    void Awake()
    {
        QuantumEvent.Subscribe<EventOnTrafficLightStateChanged>(this, OnTrafficLightStateChanged);

        m_currentRotation = m_targetRotation = IDLE;
    }

    private void Update()
    {
        m_time += Time.deltaTime * RotationSpeed;
        transform.localRotation = Quaternion.Lerp(m_currentRotation, m_targetRotation, m_time);
    }

    private void OnTrafficLightStateChanged(EventOnTrafficLightStateChanged e)
    {
        if (e.Game.Frames.Verified.Global->CurrentLightState == TrafficLightState.Green)
        {
            m_currentRotation = RED_LIGHT;
            m_targetRotation = IDLE;
            m_time = 0f;
        }
        else if (e.Game.Frames.Verified.Global->CurrentLightState == TrafficLightState.Amber)
        {
            m_currentRotation = IDLE;
            m_targetRotation = RED_LIGHT;
            m_time = 0f;
        }
    }
}
