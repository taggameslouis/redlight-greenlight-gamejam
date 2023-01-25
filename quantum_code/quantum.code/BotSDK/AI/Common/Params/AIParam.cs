using System;

namespace Quantum
{
  public enum AIParamSource
  {
    None,
    Value,
    Config,
    Blackboard,
  }

  [Serializable]
  public abstract unsafe class AIParam<T>
  {
    public AIParamSource Source = AIParamSource.Value;
    public string Key;
    public T DefaultValue;

    public unsafe T Resolve(Frame frame, AIBlackboardComponent* blackboardComponent, AIConfig configData)
    {
      if (Source == AIParamSource.Value || string.IsNullOrEmpty(Key) == true)
        return DefaultValue;

      switch (Source)
      {
        case AIParamSource.Blackboard:
          BlackboardValue blackboardValue = blackboardComponent->GetBlackboardValue(frame, Key);
          return GetBlackboardValue(blackboardValue);

        case AIParamSource.Config:
          AIConfig.KeyValuePair config = configData != null ? configData.Get(Key) : null;
          return config != null ? GetConfigValue(config) : DefaultValue;
      }

      return default(T);
    }

    protected abstract T GetBlackboardValue(BlackboardValue value);
    protected abstract T GetConfigValue(AIConfig.KeyValuePair config);
  }
}
