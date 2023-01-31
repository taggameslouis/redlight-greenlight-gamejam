using TMPro;
using Quantum;

public unsafe class InitialCountdownView : QuantumCallbacks
{
    public TextMeshProUGUI Text;

    private int m_lastTime = -1;
    
    void Awake()
    {
        QuantumEvent.Subscribe<EventOnGameStateChanged>(this, OnGameStateChanged);
        Text.gameObject.SetActive(false);
    }

    private void OnGameStateChanged(EventOnGameStateChanged callback)
    {
        if (callback.NewGameState == GameState.Starting)
        {
            Text.gameObject.SetActive(true);
            AudioManager.Instance.Play("countdown");
        }
        else
        {
            Text.gameObject.SetActive(false);
        }
    }

    public override void OnSimulateFinished(QuantumGame game, Frame frame)
    {
        base.OnSimulateFinished(game, frame);
        
        UpdateTime(frame);
    }

    private void UpdateTime(Frame frame)
    {
        if (frame.Global->CurrentGameState != GameState.Starting)
            return;
        
        var time = Photon.Deterministic.FPMath.CeilToInt(frame.Global->InitialCountdown);

        if (m_lastTime != time)
        {
            Text.text = $"{time}";
            m_lastTime = time;
        }
    }
}
