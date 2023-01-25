using Quantum.Demo;
using UnityEngine;
using UnityEngine.UI;

public sealed class SelectRegionButton : MonoBehaviour
{
    public Button Button;
    public PhotonRegions.RegionInfo Region;
    public System.Action<PhotonRegions.RegionInfo> onClick;
    public int ButtonIndex;

    private LobbyController _lobbyControler;
    private Animator Animator;

    private void Reset()
    {
        Button = GetComponent<Button>();
    }

    private void Awake()
    {
        _lobbyControler = GetComponentInParent<LobbyController>();
        Animator = GetComponent<Animator>();
        Button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        onClick?.Invoke(Region);
    }

    private void Update()
    {
        if (_lobbyControler == null) return;

        if (_lobbyControler.CurrentRegion == ButtonIndex)
            Animator.SetTrigger("Play");
        else
            Animator.SetTrigger("Stop");
    }
}