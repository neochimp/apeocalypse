using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType{
  Closest,      //default behavior
  Farthest,     //not implemented
  LowestHealth,
  HighestHealth, //not implemented
  MostDamaged
}


public class UnitController : MonoBehaviour
{

    [Header("Target")]
    public string targetTag = "Enemy"; //tag for targets, gorilla for humans, human for gorillas

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Health")]
    public float maxHealth = 100;
    public float currentHealth;


    [Header("Animation Frames")]
    public Sprite[] idleFrames;
    public Sprite[] walkFrames;
    public Sprite[] attackFrames;
    public float frameRate = 0.1f;
    public bool spriteFacesLeft = false;

    public AttackBehavior attackBehavior; //unique to each unit
    public TargetType targetBehavior;

    private Rigidbody2D rb;
    private Transform target;
    private SpriteRenderer spriteRenderer;

    private bool isMoving;
    private bool isAttacking;
    private float attackAnimTimer;
    private int attackFrame;
    private int currentFrame;
    private float timer;
   
    void Awake(){
      rb = GetComponent<Rigidbody2D>();
      rb.gravityScale = 0f;
      spriteRenderer = GetComponentInChildren<SpriteRenderer>();


      //first idle frame
      if(idleFrames != null && idleFrames.Length > 0){
        spriteRenderer.sprite = idleFrames[0];
      }

      currentHealth = maxHealth;
      if(!attackBehavior){
        attackBehavior = GetComponent<AttackBehavior>(); //default attack behavior if one isn't found
      }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(target == null || !target.gameObject.activeInHierarchy){
        target = FindNearestEnemy();
      }

      if(!target){
        rb.velocity = Vector2.zero;
        SetMoving(false);
        return;
      }
      float dist = Vector2.Distance(transform.position, target.position);
      float desiredRange = attackBehavior ? attackBehavior.Range : 0.5f;

      if(dist > desiredRange){

        Vector2 dir = (target.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed;

        if(spriteRenderer && Mathf.Abs(dir.x) > 0.01f){
          bool movingRight = dir.x > 0f;
          spriteRenderer.flipX = (spriteFacesLeft != movingRight);
        } 

        SetMoving(true);
      }else{
        Vector2 dir = (target.position - transform.position).normalized;
        if(Mathf.Abs(dir.x) > 0.01f){
          spriteRenderer.flipX = (spriteFacesLeft != dir.x > 0f);
        }
        rb.velocity = Vector2.zero;
        attackBehavior.Tick(Time.deltaTime);
        attackBehavior.TryAttack(target);
        SetMoving(false);
      }

    }

    void LateUpdate(){
      UpdateAnimation();
    }

    public void TakeDamage(float amount){
      StartCoroutine(Flash(amount, Color.red));
      if(currentHealth <= 0) Die();
    }

    public void HealDamage(float amount){
      if(currentHealth < maxHealth){  
        StartCoroutine(Flash(amount, Color.green));
      }
      if(currentHealth > maxHealth){
        currentHealth = maxHealth;
      }
    }

    void Die(){
      gameObject.SetActive(false);
    }

    Transform FindNearestEnemy(){
      GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
      if (enemies == null) return null;
      Transform nearest = null;
      float best = Mathf.Infinity;

      foreach(var e in enemies){
        if(targetBehavior == TargetType.Closest){
          float d = Vector2.Distance(transform.position, e.transform.position);
          if(d < best){
            best = d;
            nearest = e.transform;
          }
        }

        if(targetBehavior == TargetType.LowestHealth){
          var enemy = e.GetComponent<UnitController>();
          if(enemy.currentHealth < best){
            best = enemy.currentHealth;
            nearest = e.transform;
          }
        }

        if(targetBehavior == TargetType.MostDamaged){
          var enemy = e.GetComponent<UnitController>();
          if((enemy.currentHealth/enemy.maxHealth) < best){
            best = (enemy.currentHealth/enemy.maxHealth);
            nearest = e.transform;
          }
        }
      }
      
      //Debug.Log("Nearest is: " + nearest.name);
      return nearest;
    }

    void SetMoving(bool moving){
      if(isMoving == moving) return;
      
      isMoving = moving;
      currentFrame = 0;
      timer = 0;
    
      Sprite[] frames = isMoving ? walkFrames : idleFrames;
      if(spriteRenderer && frames != null && frames.Length > 0){
        spriteRenderer.sprite = frames[0];
      }
    }

    void UpdateAnimation(){
      Sprite[] frames;

      if(isAttacking){
        frames = attackFrames;
        attackAnimTimer += Time.deltaTime;

        if(attackAnimTimer >= frameRate){
          attackAnimTimer -= frameRate;
          attackFrame++;

          if(attackFrame >= attackFrames.Length){
            isAttacking = false;
            attackFrame = 0;
            timer = 0f;
            currentFrame = 0;
            spriteRenderer.sprite = idleFrames[0];
            return;
          }
          spriteRenderer.sprite = attackFrames[attackFrame];
        }
        return;

      }

      frames = isMoving ? walkFrames : idleFrames;
      if(frames == null || frames.Length == 0) return;

      timer += Time.deltaTime;
      if(timer >= frameRate){
        timer = 0;
        currentFrame = (currentFrame + 1) % frames.Length;
        spriteRenderer.sprite = frames[currentFrame];
      }
    }
    public void StartAttackAnimation(){
      if(isAttacking) return;
      if (spriteRenderer == null || attackFrames == null || attackFrames.Length == 0) return;
      if(attackFrames == null || attackFrames.Length == 0) return;
      isAttacking = true;
      attackAnimTimer = 0f;
      attackFrame = 0;

      spriteRenderer.sprite = attackFrames[0];
    }

    IEnumerator Flash(float amount, Color color){
      yield return new WaitForSeconds(0.25f);
      currentHealth -= amount;
      spriteRenderer.color = color;
      yield return new WaitForSeconds(0.1f);
      spriteRenderer.color = Color.white;

    }

}
