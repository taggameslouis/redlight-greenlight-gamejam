using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.InputSystem;

public unsafe class LocalInput : MonoBehaviour
{
	private PlayerInput _playerInput;

	private void Start()
	{
		QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
		_playerInput = GetComponent<PlayerInput>();
	}

	public void PollInput(CallbackPollInput callback)
	{
		var playerInput = new Quantum.Input();
		playerInput.Direction = _playerInput.actions["Move"].ReadValue<Vector2>().ToFPVector2();
		callback.SetInput(playerInput, DeterministicInputFlags.Repeatable);
	}
}
