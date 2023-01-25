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
		Quantum.Input i = new Quantum.Input();

		FPVector2 directional = _playerInput.actions["Move"].ReadValue<Vector2>().ToFPVector2();

		var f = callback.Game.Frames.Verified;
		if (f.Global->IsFirstMatch == false)
		{
			directional *= -1;
		}
		i.Direction = directional;

		i.Kick = _playerInput.actions["Kick"].IsPressed();
		i.SwitchCharacter = _playerInput.actions["Switch"].IsPressed();
		i.Sprint = _playerInput.actions["Sprint"].IsPressed();
		i.Slide = _playerInput.actions["Slide"].IsPressed();
		i.Pass = _playerInput.actions["Pass"].IsPressed();

		callback.SetInput(i, DeterministicInputFlags.Repeatable);
	}
}
