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
    public bool spriteFacesLeft = false;  //used to correcet left and right facing depending on sprite

    [Header("Controllers")]
    public AttackBehavior attackBehavior; //unique to each unit
    public TargetType targetBehavior; //see enum above
    public Coin coinController; //used for gorilla coin drops

    private Rigidbody2D rb;
    private Transform target;
    private SpriteRenderer spriteRenderer;
    private bar healthBar;

    //status tracking
    private bool isMoving;
    private bool isAttacking;
    private float attackAnimTimer;  //timer for attack animation
    private int attackFrame;
    private int currentFrame;
    private float timer;

    public Vector2 assignedTile;
    public Vector2 tilePosition;

    private Main mainController; 
   
    //basic instantiations
    void Awake(){
      mainController = Camera.main.GetComponent<Main>();
      rb = GetComponent<Rigidbody2D>();
      rb.gravityScale = 0f;
      spriteRenderer = GetComponentInChildren<SpriteRenderer>();
      healthBar = GetComponentInChildren<bar>();
      healthBar.MaxValue = maxHealth;
      healthBar.Value = maxHealth;
      currentHealth = maxHealth;

      //first idle frame
      if(idleFrames != null && idleFrames.Length > 0){
        spriteRenderer.sprite = idleFrames[0];
      }

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
      //checking if unit is still alive
      if(currentHealth <= 0) Die();
      //finding target
      target = FindNearestEnemy();
      attackBehavior.SetTarget(target);

      //if there are no targets stop moving for gorillas, go home for humans
      if(!target || !mainController.roundOn){
        //if the unit has an assigned tile
        if(assignedTile != null){
          //go to the assigned position/ stop if close enough
          Vector2 dir = (tilePosition - (Vector2)transform.position).normalized;
          if(Vector2.Distance((Vector2)transform.position, tilePosition) <= 0.05f){
            rb.velocity = Vector2.zero;
            SetMoving(false);
          }else{
            rb.velocity = dir * moveSpeed;
            SetMoving(true);
            //code for flipping sprite to walking direction
            if(spriteRenderer && Mathf.Abs(dir.x) > 0.01f){
              bool movingRight = dir.x > 0f;
              spriteRenderer.flipX = (spriteFacesLeft != movingRight);
            }
          }
          return;
        }else{
          //if no assigned position go to the center. 
          rb.velocity = new Vector2(1.5f,1f);
          SetMoving(false);
          return;
        }
      }
      //behavior for stopping when in attack range
      float dist = Vector2.Distance(transform.position, target.position);
      float desiredRange = attackBehavior ? attackBehavior.Range : 0.5f;

      //only attack if round is active
      if(mainController.roundOn){
        //Get the direction unit needs to go for the target
        Vector2 dir = (target.position - transform.position).normalized;
        //if unit is farther than desired range
        if(dist > desiredRange){
          //go towards the target
          rb.velocity = dir * moveSpeed;
          //make sprite face direction its walking
          if(spriteRenderer && Mathf.Abs(dir.x) > 0.01f){
            bool movingRight = dir.x > 0f;
            spriteRenderer.flipX = (spriteFacesLeft != movingRight);
          } 
          SetMoving(true);
          //if unit is in range
        }else{
          if(Mathf.Abs(dir.x) > 0.01f){
            spriteRenderer.flipX = (spriteFacesLeft != dir.x > 0f);
          }
          //stop moving
          rb.velocity = Vector2.zero;
          //try attacking
          attackBehavior.TryAttack(target, this.GetComponent<AudioSource>());
          SetMoving(false);
        }
        //tick time for attack cooldown
        attackBehavior.Tick(Time.deltaTime);
      }
    }

    void LateUpdate(){
      UpdateAnimation();
    }

    //the unit takes damage and flashes red
    public void TakeDamage(float amount){
      StartCoroutine(Flash(amount, Color.red));
      healthBar.Change(-amount);
    }

    //heal the unit and flash green
    public void HealDamage(float amount){
      if(currentHealth < maxHealth){  
        StartCoroutine(Flash(amount, Color.green));
        healthBar.Change(-amount);
      }
      //do not let health exceed max
      if(currentHealth > maxHealth){
        currentHealth = maxHealth;
      }
      
    }

    void Die(){
      if(this.gameObject.tag == "Gorilla"){
        coinController.SpawnCoin(transform.position);
      }
      Destroy(gameObject);
    }

    //unit targeting behavior
    Transform FindNearestEnemy(){
      GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
      if (enemies == null) return null;
      Transform nearest = null;
      float best = Mathf.Infinity;

      //targets the closest enemy in viscinity
      foreach(var e in enemies){
        Renderer eRenderer = e.GetComponent<Renderer>();
        if(!eRenderer.isVisible){
          continue;
        }
        if(targetBehavior == TargetType.Closest){
          float d = Vector2.Distance(transform.position, e.transform.position);
          if(d < best){
            best = d;
            nearest = e.transform;
          }
        }
        
        //targets enemy with the lowest current health
        if(targetBehavior == TargetType.LowestHealth){
          var enemy = e.GetComponent<UnitController>();
          if(enemy.currentHealth < best){
            best = enemy.currentHealth;
            nearest = e.transform;
          }
        }

        //targets unit with the most sustained damage (used for healing mostly)
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

    //assign a new tile to the unit
    public void AssignTile(int x, int y){
      assignedTile = new Vector2(x, y);
      
      tilePosition = new Vector2(x-7.5f,y-4f);
      //Debug.Log("I should now move to: " +assignedTile);
    }

    //getters for the currently assigned tile. used for releasing old tile in 2d array
    public int oldX(){
      return (int)assignedTile.x;
    }
    public int oldY(){
      return (int)assignedTile.y;
    }


//--------------------------------------Animation stuff--------------------------------
    //used for walking animation
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

    //used for refreshing between attacking and walking and idle animations
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

    //updates health and makes unit flash color.
    IEnumerator Flash(float amount, Color color){
      yield return new WaitForSeconds(0.25f);
      currentHealth -= amount;
      spriteRenderer.color = color;
      yield return new WaitForSeconds(0.1f);
      spriteRenderer.color = Color.white;

    }

}
