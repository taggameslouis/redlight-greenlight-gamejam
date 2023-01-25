using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Quantum;

public unsafe class ScoreBoardView : QuantumCallbacks
{
    public TextMeshProUGUI ScoreTeamA;
    public TextMeshProUGUI ScoreGoalAnimationTeamA;
    public TextMeshProUGUI ScoreTeamB;
    public TextMeshProUGUI ScoreGoalAnimationTeamB;
    public GameObject GoalAnimation;

    private void Start()
    {
        QuantumEvent.Subscribe<EventOnGoal>(this, OnGoal);
    }

    private void OnGoal(EventOnGoal e)
    {
        GoalAnimation.SetActive(false);
        GoalAnimation.SetActive(true);
    }

    public override void OnSimulateFinished(QuantumGame game, Frame f)
    {
        /*ScoreTeamA.text = f.Global->Players[0].PlayerScore.ToString();
        ScoreGoalAnimationTeamA.text = f.Global->Players[0].PlayerScore.ToString();
    
        ScoreTeamB.text = f.Global->Players[1].PlayerScore.ToString();
        ScoreGoalAnimationTeamB.text = f.Global->Players[1].PlayerScore.ToString();*/
    }
}