using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMeleeAttack : AttackBehavior{
  public float damage = 1f;
  public float lungeDistance = 0.25f;
  public float lungeSpeed = 10f;
  public float returnSpeed = 10f;

  protected override void PerformAttack(Transform target){
    var enemy = target.GetComponent<UnitController>();
    var controller = GetComponent<UnitController>();
    StartCoroutine(Lunge(target.position));
    if(enemy) enemy.TakeDamage(damage);
    //Debug.Log("Attacking " + enemy.name);
  }

  private IEnumerator Lunge(Vector3 targetPos){
    Vector3 startPos = transform.position;

    Vector3 direction = (targetPos - startPos).normalized;
    direction.y = 0f;

    Vector3 lungePos = startPos + direction*lungeDistance;

    float t = 0f;
    while (t < 1f){
      t += Time.deltaTime * lungeSpeed;
      transform.position = Vector3.Lerp(startPos, lungePos, t);
      yield return null;
    }

    t = 0f;
    while(t < 1f){
      t += Time.deltaTime * returnSpeed;
      transform.position = Vector3.Lerp(lungePos, startPos, t);
      yield return null;
    }
  }
}
