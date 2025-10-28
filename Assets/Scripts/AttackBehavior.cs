using UnityEngine;

public abstract class AttackBehavior : MonoBehaviour{

  [Header("Attack")]
  public float Range = 0.5f;
  public float Cooldown = 1f;
  protected float cdTimer;
  protected Transform target;
  
  public void Tick(float dt){
    if(cdTimer > 0f) cdTimer -= dt;
    if(cdTimer < 0f) cdTimer = 0f;
  }

  public bool TryAttack(Transform target, AudioSource audio){ 
    if(cdTimer > 0f || !target) return false;
    if(!IsInRange(target)) return false;

    PerformAttack(target);
    audio.Play();
    cdTimer = Cooldown;
    //Debug.Log($"ATTACK fired. Next ready in {Cooldown}s");

    var controller = GetComponent<UnitController>();
    if(controller) controller.StartAttackAnimation();
    
    return true;
  }

  public void SetTarget(Transform received){
    target = received;
  }

  protected virtual bool IsInRange(Transform target){
    return (transform.position - target.position).sqrMagnitude <= Range * Range;
  }

  protected abstract void PerformAttack(Transform target);

}
