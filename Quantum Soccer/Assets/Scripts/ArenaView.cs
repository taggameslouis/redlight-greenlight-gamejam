using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaView : MonoBehaviour
{
  public bool IsFirstMatch = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (IsFirstMatch == false)
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
    }
    else
    {
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    }
  }
}
