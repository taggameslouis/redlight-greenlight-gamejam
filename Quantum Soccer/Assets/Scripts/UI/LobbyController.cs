using Quantum.Demo;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;

    public LobbyFlow Flow;
    public SelectRegionButton[] Buttons;

    public GameObject SelectRegionsPanel;
    public GameObject SearchingScreen;
    public GameObject TransitionScreen;
    public GameObject Environment;
    public GameObject PlayButton;
    public int CurrentRegion = 0;

    private PlayerInput _playerInput;


    public void Reset()
    {
        Buttons = GetComponentsInChildren<SelectRegionButton>();
        foreach (var button in Buttons) button.Button.interactable = true;
        Flow.Started = false;
        SelectRegionsPanel.SetActive(false);
        SearchingScreen.SetActive(false);
        TransitionScreen.SetActive(false);

        Environment.SetActive(true);
        PlayButton.SetActive(true);
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        foreach (var button in Buttons) button.onClick += OnRoomButtonClick;

        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        UpdateCurrentRegion();

        if (LobbyFlow.Client == null) return;

        if (LobbyFlow.Client.InRoom && LobbyFlow.Client.CurrentRoom.PlayerCount > 1)
        {
            SearchingScreen.SetActive(false);
            TransitionScreen.SetActive(true);
            Environment.SetActive(false);
            PlayButton.SetActive(false);
        }
    }

    private void UpdateCurrentRegion()
    {
        if (SelectRegionsPanel.activeSelf && _playerInput.actions["Enter"].IsPressed())
            Buttons[CurrentRegion].OnClick();

        if (_playerInput.actions["SelectUp"].IsPressed())
            if (CurrentRegion > 0)
                CurrentRegion--;
        if (_playerInput.actions["SelectDown"].IsPressed())
            if (CurrentRegion < Buttons.Length - 1)
                CurrentRegion++;
    }

    public void OnRoomButtonClick(PhotonRegions.RegionInfo region)
    {
        foreach (var button in Buttons) button.Button.interactable = false;
        SelectRegionsPanel.SetActive(false);
        Flow.LoadRoom(region);
        SearchingScreen.SetActive(true);
    }
}