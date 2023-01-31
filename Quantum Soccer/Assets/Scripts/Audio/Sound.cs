using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)] public float Volume;
    public float StartTime = 0f;

    public AudioSource Source;
    public bool Loop;
}