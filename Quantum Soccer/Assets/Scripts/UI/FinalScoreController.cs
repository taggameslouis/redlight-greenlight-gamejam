using UnityEngine;
using Quantum;
using TMPro;

public class FinalScoreController : MonoBehaviour
{
    public GameObject FinalScore;
    public TextMeshProUGUI ScoreTextTeamA;
    public TextMeshProUGUI ScoreTextTeamB;
    public TextMeshProUGUI WinnerLooserText;

    private void Start()
    {
        QuantumEvent.Subscribe<EventOnGameStateChanged>(this, OnGameEnd);
    }

    private void OnGameEnd(EventOnGameStateChanged e)
    {
        if (e.NewGameState == GameState.Ended)
        {
            var scoreA = 99; //f.Global->Players[0].PlayerScore;
            var scoreB = 99; //f.Global->Players[1].PlayerScore;
            ScoreTextTeamA.text = scoreA.ToString();
            ScoreTextTeamB.text = scoreB.ToString();
            FinalScore.SetActive(true);

            if (scoreA != scoreB)
            {
                var winner = scoreA > scoreB ? 0 : 1;
                if (e.Game.PlayerIsLocal(winner))
                    WinnerLooserText.text = "You Won";
                else
                    WinnerLooserText.text = "You Lose";
            }
            else
            {
                WinnerLooserText.text = "Draw";
            }
        }
    }

    public void OnLeaveGame()
    {
    }
}