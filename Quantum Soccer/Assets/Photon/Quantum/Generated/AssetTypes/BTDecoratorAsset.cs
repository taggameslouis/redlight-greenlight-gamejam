// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial  
// declarations in another file.
// </auto-generated>

using Quantum;
using UnityEngine;

public abstract partial class BTDecoratorAsset : BTNodeAsset {

}

public static partial class BTDecoratorAssetExts {
  public static BTDecoratorAsset GetUnityAsset(this BTDecorator data) {
    return data == null ? null : UnityDB.FindAsset<BTDecoratorAsset>(data);
  }
}
