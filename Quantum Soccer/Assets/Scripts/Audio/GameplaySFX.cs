using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;

public class GameplaySFX : MonoBehaviour
{
    private void Start()
    {
        QuantumEvent.Subscribe<EventCharacterKick>(this, OnCharacterKick);
        QuantumEvent.Subscribe<EventCharacterPass>(this, OnCharacterPass);
        QuantumEvent.Subscribe<EventCharacterCaptureBall>(this, OnCharacterCaptureBall);
        //QuantumEvent.Subscribe<EventBallBounce>(this, OnBallBounce);
        //QuantumEvent.Subscribe<EventBallCollide>(this, OnBallCollide);
        QuantumEvent.Subscribe<EventOnGoal>(this, OnGoal);
        QuantumEvent.Subscribe<EventCharacterSprint>(this, OnSprint);
        QuantumEvent.Subscribe<EventCharacterSlide>(this, OnSlide);
        QuantumEvent.Subscribe<EventCharacterSwitch>(this, OnSwitchCharacter);
        QuantumEvent.Subscribe<EventOnGameEnd>(this, OnGameEnd);
    }

    private void OnGameEnd(EventOnGameEnd e)
    {
        AudioManager.Instance.Play("whistle_end");
        AudioManager.Instance.Play("end_game_crowd");
    }

    private void OnSwitchCharacter(EventCharacterSwitch e)
    {
        if (QuantumRunner.Default.Game.PlayerIsLocal(e.player)) AudioManager.Instance.Play("switch");
    }

    private void OnSlide(EventCharacterSlide e)
    {
        AudioManager.Instance.Play("slide");
    }

    private void OnSprint(EventCharacterSprint e)
    {
        AudioManager.Instance.Play("sprint");
    }

    private void OnGoal(EventOnGoal e)
    {
        AudioManager.Instance.Play("goal");
        AudioManager.Instance.Play("whistle_start");
    }
    //
    // private void OnBallCollide(EventBallCollide e)
    // {
    //     AudioManager.Instance.Play("pass");
    // }
    //
    // private void OnBallBounce(EventBallBounce e)
    // {
    //     AudioManager.Instance.Play("ball_touch");
    // }

    private void OnCharacterKick(EventCharacterKick e)
    {
        AudioManager.Instance.Play("kick");
    }

    private void OnCharacterPass(EventCharacterPass e)
    {
        AudioManager.Instance.Play("pass");
    }

    private void OnCharacterCaptureBall(EventCharacterCaptureBall e)
    {
        AudioManager.Instance.Play("pass");
    }

    private void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }
}