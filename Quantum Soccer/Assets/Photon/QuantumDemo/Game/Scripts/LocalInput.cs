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
		callback.SetInput(i, DeterministicInputFlags.Repeatable);
	}
}
