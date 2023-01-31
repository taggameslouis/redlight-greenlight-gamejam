using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;
using UnityEngine.UI;

public unsafe class CharacterView : QuantumCallbacks
{
    public EntityView EntityView;
    public GameObject ControlledIndicator;
    public Animator Animator;
    public Image StaminaIndicator;
    public RectTransform CharacterCanvas;
    public GameObject Arrow;
    public TMPro.TextMeshProUGUI Nickname;

    public void Awake()
    {
        //QuantumEvent.Subscribe<EventCharacterFall>(this, OnCharacterFall);
        //QuantumEvent.Subscribe<EventCharacterSlide>(this, OnCharacterSlide);
        //QuantumEvent.Subscribe<EventCharacterKick>(this, OnCharacterKick);
        QuantumEvent.Subscribe<EventCharacterStateChanged>(this, OnCharacterStateChanged);
        
        EntityView.OnEntityInstantiated.AddListener(OnEntityInstantiated);
    }

    private void OnEntityInstantiated(QuantumGame game)
    {
        var f = QuantumRunner.Default.Game.Frames.Verified;
        if (!f.TryGet<CharacterFields>(EntityView.EntityRef, out var characterFields))
            return;

        if (QuantumRunner.Default.Game.PlayerIsLocal(characterFields.Player))
        {
            var cameraBehaviour = GameObject.FindObjectOfType<CameraBehaviour>();
            cameraBehaviour.FocusTarget = gameObject;
            Debug.Log("[CharacterView] Assigning camera target");
        }

        Nickname.text = characterFields.Nickname;
    }

    public void OnDisable()
    {
        QuantumEvent.UnsubscribeListener(this);
    }

    private void OnCharacterStateChanged(EventCharacterStateChanged e)
    {
        if (e.entity == EntityView.EntityRef)
        {
            switch (e.NewState)
            {
                case CharacterState.DEAD:
                    Animator.SetTrigger("Fall");
                    AudioManager.Instance.Play("slide");
                    break;
                
                case CharacterState.INACTIVE:
                    break;
                
                case CharacterState.ACTIVE:
                    Animator.SetTrigger("Idle");
                    break;
            }
            
        }
    }
    
/*
    private void OnCharacterKick(EventCharacterKick e)
    {
        if (e.character == EntityView.EntityRef) Animator.SetTrigger("Kick");
    }

    private void OnCharacterSlide(EventCharacterSlide e)
    {
        if (e.character == EntityView.EntityRef) Animator.SetTrigger("Slide");
    }

    private void OnCharacterFall(EventCharacterFall e)
    {
        if (e.character == EntityView.EntityRef) Animator.SetTrigger("Fall");
    }*/

    private void Update()
    {
        CharacterCanvas.rotation = Quaternion.identity;
    }

    public override void OnSimulateFinished(QuantumGame game, Frame f)
    {
        if (StaminaIndicator == null || ControlledIndicator == null) return;

        if (f.IsVerified == false) return;
        if (EntityView.EntityRef == EntityRef.None) return;

        if (Animator != null)
        {
            var kcc = f.Get<KCC>(EntityView.EntityRef);
            Animator.SetFloat("Velocity", (float)kcc.Velocity.Magnitude);
        }

        var fields = f.Get<CharacterFields>(EntityView.EntityRef);
        UpdateArrow(f, fields);

        if (game.PlayerIsLocal(fields.Player) == false)
        {
            if (ControlledIndicator != null) ControlledIndicator.SetActive(false);
            StaminaIndicator.fillAmount = 0;
            return;
        }

        /*if (EntityView.EntityRef == f.Global->Players[fields.Player].ControlledCharacter)
        {
            if (ControlledIndicator != null)
            {
                ControlledIndicator.SetActive(true);
            }
        }
        else
        {
            if (ControlledIndicator != null)
            {
                ControlledIndicator.SetActive(false);
            }

        if (EntityView.EntityRef == f.Global->Players[0].ControlledCharacter
            || EntityView.EntityRef == f.Global->Players[1].ControlledCharacter)
        {
            StaminaIndicator.fillAmount = (float)fields.Stamina;
        }
        else
        {
            StaminaIndicator.fillAmount = 0;
        }
        }*/
    }

    private void UpdateArrow(Frame f, CharacterFields fields)
    {
       /* if (f.Global->BallOwner != EntityView.EntityRef)
        {
            Arrow.SetActive(false);
            return;
        }
        else
        {
            Arrow.SetActive(true);
        }*/

        /*var player = f.Global->Players.GetPointer(fields.Player);
        Arrow.transform.localScale = Vector3.one * (2 + player->HoldKickTimer.AsFloat * 2);*/
    }
}