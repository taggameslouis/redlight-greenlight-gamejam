using UnityEngine;
using Quantum;

public class TopBarView : MonoBehaviour
{
    public float TransitionSpeed = 5f;

    private Vector3 m_startPosition;
    private Vector3 m_defaultPosition;   
    private bool m_transitionIn = false;
    private float m_transitionTime = 0f;
    
    void Awake()
    {
        m_defaultPosition = transform.position;
        m_startPosition = m_defaultPosition + new Vector3(0, 100, 0);
        transform.position = m_startPosition;
        QuantumEvent.Subscribe<EventOnGameStateChanged>(this, OnGameStateChanged);
    }

    private void OnGameStateChanged(EventOnGameStateChanged callback)
    {
        if (callback.NewGameState == GameState.Running)
        {
            m_transitionIn = true;
            m_transitionTime = 0f;
        }
        else
        {
            transform.position = m_startPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_transitionIn)
        {
            m_transitionTime += Time.deltaTime * TransitionSpeed;
            transform.position = Vector3.Lerp(m_startPosition, m_defaultPosition, m_transitionTime);
            if (m_transitionTime > 1f)
                m_transitionIn = false;
        }
    }
}
