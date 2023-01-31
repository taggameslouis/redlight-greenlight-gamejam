using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float FadeSpeed = 5f;

    public Sound[] sounds;

    private Coroutine m_fadeOut = null;

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
        if (m_fadeOut != null)
        {
            StopCoroutine(m_fadeOut);
            m_fadeOut = null;
        }

        var s = Array.Find(sounds, sound => sound.Name == name);
        s.Source.volume = s.Volume;
        s.Source.time = s.StartTime;
        s.Source.Play();
    }

    public void Stop()
    {
        m_fadeOut = StartCoroutine(StartFadeOut());
    }

    private IEnumerator StartFadeOut()
    {
        var s = Array.Find(sounds, sound => sound.Source.isPlaying);
        while (true)
        {
            s.Source.volume -= Time.deltaTime * FadeSpeed;
            if (s.Source.volume > 0f)
                yield return null;
            
            s.Source.Stop();
            break;
        }
    }
}