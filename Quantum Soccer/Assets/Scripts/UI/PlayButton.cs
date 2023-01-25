using Quantum.Demo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public sealed class PlayButton : MonoBehaviour
{
    public GameObject SelectRegionPanel;

    private PlayerInput _playerInput;

    private void Start()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
    }

    public void OnClick()
    {
        SelectRegionPanel.SetActive(true);
    }

    private void Update()
    {
        if (_playerInput.actions["Any"].IsPressed() && _playerInput.actions["Enter"].IsPressed() == false)
        {
            SelectRegionPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}