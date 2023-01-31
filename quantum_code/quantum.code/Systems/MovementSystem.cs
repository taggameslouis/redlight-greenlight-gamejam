using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
	public unsafe class MovementSystem : SystemMainThread, ISignalOnCharacterMove
	{
		public override void Update(Frame f)
		{
			if (f.Global->CurrentGameState != GameState.Running)
				return;
				
			foreach (var (entity, character) in f.Unsafe.GetComponentBlockIterator<CharacterFields>())
			{
				if (character->State != CharacterState.ACTIVE)
					continue;
				
				var input = f.GetPlayerInput(character->Player);
				f.Signals.OnCharacterMove(entity, input->Direction);
			}
		}

		public void OnCharacterMove(Frame f, EntityRef character, FPVector2 inputDirection)
		{
			var transform = f.Unsafe.GetPointer<Transform2D>(character);

			if (inputDirection != FPVector2.Zero)
			{
				var angle = FPVector2.RadiansSigned(FPVector2.Right, inputDirection);

				transform->Rotation = FPMath.LerpRadians(transform->Rotation, angle, f.DeltaTime * FP._3 * FP._10);
			}

			var direction = transform->Forward * inputDirection.Magnitude;

			MoveCharacter(f, character, direction);
		}

		private void MoveCharacter(Frame f, EntityRef character, FPVector2 direction)
		{
			var kcc = f.Unsafe.GetPointer<KCC>(character);
			var settings = f.FindAsset<KCCSettings>(kcc->Settings.Id);
			settings.Init(ref *kcc);

			var movement = settings.ComputeRawMovement(f, character, direction);
			settings.SteerAndMove(f, character, in movement);
		}
	}
}