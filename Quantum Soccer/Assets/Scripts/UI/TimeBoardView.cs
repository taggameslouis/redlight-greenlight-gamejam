using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Quantum;

public unsafe class TimeBoardView : QuantumCallbacks
{
    public TextMeshProUGUI TimeBoardText;

    public override void OnSimulateFinished(QuantumGame game, Frame f)
    {
        TimeBoardText.text = f.Global->MatchTimer.AsInt.ToString();
    }
}