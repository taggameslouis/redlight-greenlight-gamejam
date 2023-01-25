using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quantum;
using TMPro;

public unsafe class FinalScoreController : MonoBehaviour
{
  public GameObject FinalScore;
  public TextMeshProUGUI ScoreTextTeamA;
  public TextMeshProUGUI ScoreTextTeamB;
  public TextMeshProUGUI WinnerLooserText;

  void Start()
  {
    QuantumEvent.Subscribe<EventOnGameEnd>(this, OnGameEnd);
  }

  private void OnGameEnd(EventOnGameEnd e)
  {
    var f = e.Game.Frames.Verified;
    if (f.Global->State == GameState.Ended)
    {
      var scoreA = f.Global->Players[0].PlayerScore;
      var scoreB = f.Global->Players[1].PlayerScore;
      ScoreTextTeamA.text = scoreA.ToString();
      ScoreTextTeamB.text = scoreB.ToString();
      FinalScore.SetActive(true);

      if (scoreA != scoreB)
      {
        var winner = scoreA > scoreB ? 0 : 1;
        if (e.Game.PlayerIsLocal(winner))
        {
          WinnerLooserText.text = "You Won";
        }
        else
        {
          WinnerLooserText.text = "You Lose";
        }
      }
      else
      {
        WinnerLooserText.text = "Draw";
      }

    }
  }

  public void OnLeaveGame() {
  }
}
