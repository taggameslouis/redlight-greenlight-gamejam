using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quantum;
using TMPro;

public class GameResultView : MonoBehaviour
{
    public enum State
    {
        IDLE,
        FADE_IN,
        ANNOUNCE
    }
    
    public CanvasGroup m_canvasGroup;
    public Image m_background;
    public TextMeshProUGUI m_text;
    
    public float m_fadeSpeed = 2f;

    private State m_state = State.IDLE;
    private bool m_eliminated = false;

    // Start is called before the first frame update
    void Awake()
    {
        QuantumEvent.Subscribe<EventOnGameEnded>(this, OnGameEnded);
        m_canvasGroup.alpha = 0f;
        m_canvasGroup.blocksRaycasts = false;
        m_canvasGroup.interactable = false;
    }

    private void OnGameEnded(EventOnGameEnded e)
    {
        AudioManager.Instance.StopBGMusic();
        
        m_eliminated = e.IsEliminated;
        Debug.Log($"You are elimited? {m_eliminated}");
        
        m_state = State.FADE_IN;
        
        m_canvasGroup.alpha = 0f;
        m_canvasGroup.blocksRaycasts = true;
        m_canvasGroup.interactable = true;
    }

    private void Update()
    {
        switch (m_state)
        {
            default:
                break;

            case State.FADE_IN:
            {
                m_canvasGroup.alpha += Time.deltaTime * m_fadeSpeed;
                if (m_canvasGroup.alpha >= 1f)
                {
                    m_state = State.ANNOUNCE;
                }

                break;
            }

            case State.ANNOUNCE:
            {
                m_text.text = m_eliminated ? "ELIMINATED" : "GREAT SUCCESS";
                AudioManager.Instance.Play(m_eliminated ? "result_lose" : "result_win");
                
                m_state = State.IDLE;
                break;
            }
        }
        
    }
}
