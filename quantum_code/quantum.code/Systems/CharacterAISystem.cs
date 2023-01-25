using Photon.Deterministic;
using Quantum.Core;

namespace Quantum
{
  public unsafe class CharacterAISystem : SystemMainThread
  {
    public override void OnInit(Frame f)
    {

      foreach (var (character, agent) in f.Unsafe.GetComponentBlockIterator<HFSMAgent>())
      {
        var root = f.FindAsset<HFSMRoot>(agent->Data.Root.Id);
        HFSMManager.Init(f, &agent->Data, character, root);
      }
    }

    public override void Update(Frame f)
    {
      foreach (var (character, agent) in f.Unsafe.GetComponentBlockIterator<HFSMAgent>())
      {
        HFSMManager.Update(f, f.DeltaTime, &agent->Data, character);
      }
    }


  }
}
