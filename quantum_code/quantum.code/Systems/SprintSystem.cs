using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
	public unsafe class SprintSystem : SystemMainThread
	{
		public override void Update(Frame f)
		{
			var charactersFilter = f.Filter<Transform2D, CharacterFields, KCC>();
			while (charactersFilter.NextUnsafe(out var character, out var transform, out var fields, out var kcc))
			{
				var input = f.GetPlayerInput(fields->Player);
				var spec = f.FindAsset<CharacterSpec>(fields->Spec.Id);

				if (character != f.Global->Players[0].ControlledCharacter && character != f.Global->Players[1].ControlledCharacter)
				{
					if (fields->Stamina < 1)
					{
						fields->Stamina += f.DeltaTime * spec.StaminaRegenerationVelocity;
					}
					if (fields->Stamina > 1)
					{
						fields->Stamina = 1;
					}
					continue;
				}

				if (fields->IsSliding)
				{
					continue;
				}

				if (input->Sprint.IsDown && fields->Stamina > 0)
				{
					if (fields->IsSprinting == false) {
						f.Events.CharacterSprint(character);
					}
					fields->IsSprinting = true;
					var settings = f.FindAsset<KCCSettings>(spec.SprintRunSettings.Id);
					settings.Init(ref *kcc);
					fields->Stamina -= f.DeltaTime * spec.StaminaUsageVelocity;
					if (fields->Stamina < 0)
					{
						fields->Stamina = 0;
					}
				}
				else
				{
					fields->IsSprinting = false;
					var settings = f.FindAsset<KCCSettings>(spec.DefaultRunSettings.Id);
					settings.Init(ref *kcc);
					if (input->Sprint.IsDown == false)
					{
						if (fields->Stamina < 1)
						{
							fields->Stamina += f.DeltaTime * spec.StaminaRegenerationVelocity;
						}
						if (fields->Stamina > 1)
						{
							fields->Stamina = 1;
						}
					}
				}
			}
		}
	}
}