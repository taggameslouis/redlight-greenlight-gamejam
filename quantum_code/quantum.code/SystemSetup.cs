using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum {
  public static class SystemSetup {
    public static SystemBase[] CreateSystems(RuntimeConfig gameConfig, SimulationConfig simulationConfig) {
      return new SystemBase[] {
        // pre-defined core systems
        //new Core.CullingSystem2D(),
        //new Core.CullingSystem3D(),

        new Core.PhysicsSystem2D(),
        //new Core.PhysicsSystem3D(),

        //new Core.NavigationSystem(),
        new Core.EntityPrototypeSystem(),

        // user systems go here
        new GameSystem(),
        new BotSDKDebuggerSystem(),
        new CustomGravitySystem(),
        new SprintSystem(),
        new CharacterSlideSystem(),
        new PassSystem(),
        new MovementSystem(),
        new RespawnSystem(),
        new CaptureBallSystem(),
        new KickSystem(),
        new SwitchCharacterSystem(),
        new GoalSystem(),
        new CharacterFallSystem(),
        new CharacterAISystem(),
      };
    }
  }
}