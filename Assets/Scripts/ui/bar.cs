using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bar : MonoBehaviour
{
  [field:SerializeField]
  public float MaxValue;
  [field:SerializeField]
  public float Value;
  
  [SerializeField]
  private RectTransform topBar;

  [SerializeField]
  private RectTransform bottomBar;
  private float animationSpeed = 10f;

  private float fullWidth;
  private float targetWidth => Value * fullWidth / MaxValue;

  private Coroutine adjustBarWidthCoroutine;

  private void Start(){
    fullWidth = topBar.rect.width;
  }

  private void Update(){
    if(Input.GetKeyDown(KeyCode.Minus)){
      Change(-20);
    }
    if(Input.GetKeyDown(KeyCode.Equals)){
      Change(20);
    }
  }

  private IEnumerator AdjustBarWidth(float amount){
    var suddenChangeBar = amount >= 0 ? bottomBar : topBar;
    var slowChangeBar = amount >= 0 ? topBar : bottomBar;
    suddenChangeBar.sizeDelta = new Vector2(targetWidth, suddenChangeBar.rect.height);
    while(Mathf.Abs(suddenChangeBar.rect.width - slowChangeBar.rect.width) > 1f){
      slowChangeBar.sizeDelta = new Vector2(Mathf.Lerp(slowChangeBar.rect.width, targetWidth, Time.deltaTime * animationSpeed), slowChangeBar.rect.height);
      yield return null;
    }  
    slowChangeBar.sizeDelta = new Vector2(targetWidth, slowChangeBar.rect.height);
  }

  public void Change(float amount){
    Value = Mathf.Clamp(Value + amount, 0, MaxValue);
    if(adjustBarWidthCoroutine != null){
      StopCoroutine(adjustBarWidthCoroutine);
    }

    adjustBarWidthCoroutine = StartCoroutine(AdjustBarWidth(amount));
  }

  
}
