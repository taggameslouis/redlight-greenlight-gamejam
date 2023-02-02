using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float FadeSpeed = 5f;

    public Sound[] sounds;

    private Sound m_backgroundMusic;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (var s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.loop = s.Loop;
        }
    }

    public void Play(string name)
    {
        var s = Array.Find(sounds, sound => sound.Name == name);
        s.Source.volume = s.Volume;
        s.Source.time = s.StartTime;
        s.Source.Play();
    }

    public void PlayBGMusic(string name)
    {
        m_backgroundMusic = Array.Find(sounds, sound => sound.Name == name);
        Play(name);
    }

    public void StopBGMusic()
    {
        StartCoroutine(StartFadeOut());
    }

    private IEnumerator StartFadeOut()
    {
        while (true)
        {
            m_backgroundMusic.Source.volume -= Time.deltaTime * FadeSpeed;
            if (m_backgroundMusic.Source.volume > 0f)
                yield return null;

            m_backgroundMusic.Source.Stop();
            break;
        }
    }
}