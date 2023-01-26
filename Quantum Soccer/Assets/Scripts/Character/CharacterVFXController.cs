using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
using Photon.Deterministic;

public class CharacterVFXController : QuantumCallbacks
{
    public EntityView EntityView;

    public ParticleSystem PassParticles;
    public ParticleSystem KickParticles;
    public ParticleSystem CaptureBallParticles;
    public ParticleSystem FallParticles;
    public ParticleSystem SlideParticles;
    public ParticleSystem RunParticles;

    public ParticleSystem SprintParticles;

    // Start is called before the first frame update
    private void Start()
    {
        /*QuantumEvent.Subscribe<EventCharacterKick>(this, OnCharacterKick);
        QuantumEvent.Subscribe<EventCharacterPass>(this, OnCharacterPass);
        QuantumEvent.Subscribe<EventCharacterCaptureBall>(this, OnCharacterCaptureBall);
        QuantumEvent.Subscribe<EventCharacterFall>(this, OnCharacterFall);
        QuantumEvent.Subscribe<EventCharacterSlide>(this, OnCharacterSlide);*/
    }

    public override void OnSimulateFinished(QuantumGame game, Frame f)
    {
        if (f.IsVerified == false) return;
        if (f.Exists(EntityView.EntityRef) == false) return;

        var body = f.Get<KCC>(EntityView.EntityRef);
        if (body.Velocity == FPVector2.Zero)
        {
            RunParticles.Stop();
            SprintParticles.Stop();
            return;
        }

        var fields = f.Get<CharacterFields>(EntityView.EntityRef);
       /* if (fields.IsSprinting)
        {
            if (SprintParticles.isPlaying == false) SprintParticles.Play();
            RunParticles.Stop();
        }
        else
        {
            SprintParticles.Stop();
            if (RunParticles.isPlaying == false) RunParticles.Play();
        }*/
    }
/*
    private void OnCharacterPass(EventCharacterPass e)
    {
        if (e.character == EntityView.EntityRef) PassParticles.Play();
    }

    private void OnCharacterKick(EventCharacterKick e)
    {
        if (e.character == EntityView.EntityRef) KickParticles.Play();
    }

    private void OnCharacterCaptureBall(EventCharacterCaptureBall e)
    {
        if (e.character == EntityView.EntityRef) CaptureBallParticles.Play();
    }

    private void OnCharacterFall(EventCharacterFall e)
    {
        if (e.character == EntityView.EntityRef) FallParticles.Play();
    }

    private void OnCharacterSlide(EventCharacterSlide e)
    {
        if (e.character == EntityView.EntityRef) SlideParticles.Play();
    }*/
}