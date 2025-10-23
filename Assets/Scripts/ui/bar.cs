using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bar : MonoBehaviour
{
  public int MaxValue;
  public int Value;

  public void Change(int amount){
    Value = Mathf.Clamp(Value + amount, 0, MaxValue);
  }

  private void Update(){
    if(Input.GetKeyDown(KeyCode.Minus)){
      Change(-20);
    }
    if(Input.GetKeyDown(KeyCode.Equals)){
      Change(20);
    }
  }
  
}
