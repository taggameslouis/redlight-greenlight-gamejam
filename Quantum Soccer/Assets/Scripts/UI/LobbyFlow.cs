using System.Collections;
using Quantum;
using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;
using Quantum.Demo;
using UnityEngine.SceneManagement;

public sealed class LobbyFlow : MonoBehaviour
{
  public string version = "v";
  public byte maxPlayerCount = 4;
  public ClientIdProvider.Type idProvider = ClientIdProvider.Type.NewGuid;
  public AssetRefGameSpec GameSpec;
  public MapAsset Map;

  public static QuantumLoadBalancingClient Client { get; set; }
  public bool Started = false;


  private void Start()
  {
    QuantumRunner.Init();
    Client = new QuantumLoadBalancingClient(PhotonServerSettings.Instance.AppSettings.Protocol);
  }

  public void LoadRoom(PhotonRegions.RegionInfo region)
  {
    StopAllCoroutines();
    StartCoroutine(LoadRoomCoroutine(region));
  }

  public IEnumerator LoadRoomCoroutine(PhotonRegions.RegionInfo region)
  {
    var appSettings = PhotonServerSettings.CloneAppSettings(PhotonServerSettings.Instance.AppSettings);
    appSettings.FixedRegion = region.Token;

    Client.ConnectUsingSettings(appSettings);

    while (!Client.IsConnectedAndReady)
    {
      yield return null;
    }

    var roomOptions = new RoomOptions();
    roomOptions.IsVisible = true;
    roomOptions.MaxPlayers = maxPlayerCount;
    roomOptions.Plugins = new string[] { "QuantumPlugin" };

    roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
    roomOptions.CustomRoomProperties.Add("MAP", Map.Settings.Scene);

    var param = new EnterRoomParams();
    param.RoomOptions = roomOptions;
    param.Lobby = TypedLobby.Default;
    var opRandon = new OpJoinRandomRoomParams();
    Client.OpJoinRandomOrCreateRoom(opRandon, param);

  }

  private void Update()
  {
    if (Client == null)
    {
      return;
    }

    Client?.Service();

    if (Client.InRoom && !Started && Client.CurrentRoom.PlayerCount > 1)
    {
      OnJoinedRoom();
    }
  }

  private void OnJoinedRoom()
  {
    Started = true;
    if (!TryGetRoomProperty<string>("MAP", out var mapName) || string.IsNullOrEmpty(mapName))
    {
      Debug.Log("NO 'MAP' ROOM PROPERTY");
      return;
    }

    var mapGuid = Map.Settings.Guid;

    if (mapGuid.Value == 0)
    {
      Debug.LogFormat("COULD NOT FIND MAP '{0}'", mapName);
      return;
    }

    var config = new RuntimeConfig();
    config.Map.Id = mapGuid;
    config.GameSpec = GameSpec;

    var param = new QuantumRunner.StartParameters
    {
      RuntimeConfig = config,
      DeterministicConfig = DeterministicSessionConfigAsset.Instance.Config,
      ReplayProvider = null,
      GameMode = Photon.Deterministic.DeterministicGameMode.Multiplayer,
      InitialFrame = 0,
      PlayerCount = Client.CurrentRoom.MaxPlayers,
      LocalPlayerCount = 1,
      RecordingFlags = RecordingFlags.Default,
      NetworkClient = Client
    };

    Debug.Log("QuantumRunner.StartGame");

    var clientId = ClientIdProvider.CreateClientId(idProvider, Client);
    QuantumRunner.StartGame(clientId, param);
  }

  private static bool TryGetRoomProperty<T>(string key, out T value)
  {

    if (Client.CurrentRoom.CustomProperties.TryGetValue(key, out var v) && v is T)
    {
      value = (T)v;
      return true;
    }
    else
    {
      value = default(T);
      return false;
    }
  }
}
