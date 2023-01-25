using System;
using UnityEngine;

public sealed class BGMusic : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.Play("torcida_bg");
    }
}