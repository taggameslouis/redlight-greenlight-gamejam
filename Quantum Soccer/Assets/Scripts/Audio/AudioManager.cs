using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance;

  public Sound[] sounds;
  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else {  
      Destroy(gameObject);
    }

    foreach (Sound s in sounds)
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
    s.Source.Play();
  }
}
