using UnityEngine;

public class MeleeAttack : AttackBehavior{
  public float damage = 1f;

  protected override void PerformAttack(Transform target){
    var enemy = target.GetComponent<UnitController>();
    if(enemy) enemy.TakeDamage(damage);
    //Debug.Log("Attacking " + enemy.name);
  }
}
