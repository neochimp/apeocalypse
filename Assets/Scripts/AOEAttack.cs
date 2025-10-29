using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : AttackBehavior{
  public GameObject circle;
  public float damage;
  public float radius;


  protected override void PerformAttack(Transform target){
    GameObject[] enemies = GameObject.FindGameObjectsWithTag(target.tag);
    StartCoroutine(SpawnAura());
    foreach(GameObject unit in enemies){
      unit.GetComponent<UnitController>().HealDamage(damage);
    }
    return;
  }

  IEnumerator SpawnAura(){
    GameObject aura = Instantiate(circle, transform.position, Quaternion.identity);
    aura.transform.localScale = new Vector3(radius*2f, radius*2f, 1f);
    aura.GetComponent<SpriteRenderer>().color = Color.yellow;
    yield return new WaitForSeconds(0.3f);
    Destroy(aura);
  }
}


