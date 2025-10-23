using UnityEngine;

public class Projectile : MonoBehaviour{
  [Header("Stats")]
  public float moveSpeed = 3f;
  public float damage = 1f;
  public string targetTag = "Gorilla";


  [Header("Animation")]
  public Sprite[] frames;
  public float frameRate = 0.1f;

  private Rigidbody2D rb;
  private SpriteRenderer spriteRenderer;
  private float animTimer;
  private int currentFrame;
  
  void Awake(){
    rb = GetComponent<Rigidbody2D>();
    rb.gravityScale = 0f;
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    
    if (spriteRenderer != null) {
      spriteRenderer.enabled = true;
      if (frames != null && frames.Length > 0 && frames[0] != null) {
        spriteRenderer.sprite = frames[0];
      }
    }
  }
  
  void Update() {
    AnimateSprite();
  }

  private void AnimateSprite() {
    // Skip if no frames assigned
    if (frames == null || frames.Length == 0 || spriteRenderer == null) return;

    animTimer += Time.deltaTime;
    if (animTimer >= frameRate) {
      animTimer -= frameRate;
      currentFrame++;

      if (currentFrame >= frames.Length)
        currentFrame = 0; // loop

      spriteRenderer.sprite = frames[currentFrame];
    }
  } 

  void OnCollisionEnter2D(Collision2D hit){
    if(hit.gameObject.CompareTag(targetTag)){
      UnitController enemy = hit.gameObject.GetComponent<UnitController>();
      if(enemy != null){
        enemy.TakeDamage(damage);
      }
      Destroy(gameObject);
    }
  }
  
  public void trajectory(Vector2 dir){
    rb.velocity = dir * moveSpeed;
  }
  
}
