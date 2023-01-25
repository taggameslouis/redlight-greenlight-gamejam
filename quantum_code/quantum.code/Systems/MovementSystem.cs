using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
	public unsafe class MovementSystem : SystemMainThread, ISignalOnCharacterMove
	{
		public override void Update(Frame f)
		{
			var charactersFilter = f.Filter<Transform2D, CharacterFields, KCC>();
			while (charactersFilter.NextUnsafe(out var character, out var transform, out var fields, out var kcc))
			{
				if (fields->IsSliding)
				{
					continue;
				}

				if (fields->FallenTimer > 0)
				{
					continue;
				}
				var input = f.GetPlayerInput(fields->Player);
				f.Signals.OnCharacterMove(character, input->Direction);
			}
		}

		public void OnCharacterMove(Frame f, EntityRef character, FPVector2 inputDirection)
		{
			var transform = f.Unsafe.GetPointer<Transform2D>(character);
			var fields = f.Unsafe.GetPointer<CharacterFields>(character);

			if (fields->FallenTimer > 0 || fields->IsSliding)
			{
				return;
			}


			if (inputDirection != FPVector2.Zero)
			{
				var angle = FPVector2.RadiansSigned(FPVector2.Right, inputDirection);
				transform->Rotation = FPMath.LerpRadians(transform->Rotation, angle, f.DeltaTime * 15);
			}

			var direction = transform->Forward * inputDirection.Magnitude;

			MoveCharacter(f, character, CharacterDirectionConstraints(f, character, direction, inputDirection));

		}

		private void MoveCharacter(Frame f, EntityRef character, FPVector2 direction)
		{
			if (f.Global->State != GameState.Running)
			{
				direction = FPVector2.Zero;
			}
			var kcc = f.Unsafe.GetPointer<KCC>(character);
			var settings = f.FindAsset<KCCSettings>(kcc->Settings.Id);
			settings.Init(ref *kcc);

			var movement = settings.ComputeRawMovement(f, character, direction);
			settings.SteerAndMove(f, character, in movement);
		}

		private FPVector2 CharacterDirectionConstraints(Frame f, EntityRef character, FPVector2 direction, FPVector2 input)
		{
			var newDirection = direction;
			var transform = f.Unsafe.GetPointer<Transform2D>(character);
			var gameSpec = f.FindAsset<GameSpec>(f.RuntimeConfig.GameSpec.Id);
			if (transform->Position.X > 0 && input.X > 0 && transform->Position.X >= gameSpec.MaxCharacterPositionX)
			{
				transform->Position.X = gameSpec.MaxCharacterPositionX;
				newDirection = FPVector2.Zero;
			}
			if (transform->Position.X < 0 && input.X < 0 && transform->Position.X < -gameSpec.MaxCharacterPositionX)
			{
				transform->Position.X = -gameSpec.MaxCharacterPositionX;
				newDirection = FPVector2.Zero;
			}

			if (transform->Position.Y > 0 && input.Y > 0 && transform->Position.Y >= gameSpec.MaxCharacterPositionY)
			{
				transform->Position.Y = gameSpec.MaxCharacterPositionY;
				newDirection = FPVector2.Zero;
			}
			if (transform->Position.Y < 0 && input.Y < 0 && transform->Position.Y < -gameSpec.MaxCharacterPositionY)
			{
				transform->Position.Y = -gameSpec.MaxCharacterPositionY;
				newDirection = FPVector2.Zero;
			}
			return newDirection;
		}
	}
}