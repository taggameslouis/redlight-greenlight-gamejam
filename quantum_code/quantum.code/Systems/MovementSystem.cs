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
				
			foreach (var (entity, activePlayer) in f.Unsafe.GetComponentBlockIterator<ActivePlayer>())
			{
				var character = f.Get<CharacterFields>(entity);
				var input = f.GetPlayerInput(character.Player);
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

		// TODO: Can probably remove this
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