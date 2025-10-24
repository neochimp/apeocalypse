using UnityEngine;

public class RangedAttack : AttackBehavior{
  public GameObject projectile; //projectile that is shot out
  public float damage;  //damage of projectile
  [Header("Laser")] //this is used if the character has a laser sight, like on Simo Hayha
  public bool laserOn;
  public Color laserColor = Color.red;
  public float laserWidth = 0.02f;
  private LineRenderer line;
  //Transform target;

  void Start(){
    //sets the laser if needed
    if(laserOn == true){
      line = GetComponent<LineRenderer>();
      line.positionCount = 2;
      line.startWidth = laserWidth;
      line.endWidth = laserWidth;
      line.material = new Material(Shader.Find("Sprites/Default"));
      line.startColor = laserColor;
      line.endColor = laserColor;
    }

  }

  void Update(){
    //disables laser when no target
    if(target != null && laserOn == true){
      line.enabled = true;
      line.SetPosition(0, transform.position);
      line.SetPosition(1, target.position);
    }else{
      if(target == null && laserOn == true){
        line.enabled = false;
      }
    }
  }

  protected override void PerformAttack(Transform target){
    //creates new projectile
    GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
    //sets the projectiles damage
    newProjectile.GetComponent<Projectile>().damage = damage;
    //fires the projectile in the correct direction
    newProjectile.GetComponent<Projectile>().trajectory((target.position - transform.position).normalized);
  }




}
