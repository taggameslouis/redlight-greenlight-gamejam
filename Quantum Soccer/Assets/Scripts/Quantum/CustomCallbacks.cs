using System.Linq;
using UnityEngine;
using Quantum;

public class CustomCallbacks : QuantumCallbacks
{
    public AssetRefEntityPrototype Prototype;
    
    public override void OnGameStart(QuantumGame game)
    {
        // paused on Start means waiting for Snapshot
        if (game.Session.IsPaused) 
            return;

        foreach (var lp in game.GetLocalPlayers())
        {
            Debug.Log("CustomCallbacks - sending player: " + lp);
            game.SendPlayerData(lp, new RuntimePlayer
            {
                Nickname = Quantum.Demo.UIMain.Client.NickName,
                CharacterPrototype = Prototype
            });
            
            Debug.Log($"OnGameStart: Player {lp} sending nickname - {Quantum.Demo.UIMain.Client.NickName}");
        }
    }

    public override void OnGameResync(QuantumGame game)
    {
        Debug.Log("Detected Resync. Verified tick: " + game.Frames.Verified.Number);
    }
}