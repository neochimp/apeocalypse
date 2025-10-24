using UnityEngine;

public class RangedAttack : AttackBehavior{
  public GameObject projectile;
  public float damage;
  [Header("Laser")]
  public bool laserOn;
  public Color laserColor = Color.red;
  public float laserWidth = 0.02f;
  private LineRenderer line;
  //Transform target;

  void Start(){
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
    GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
    newProjectile.GetComponent<Projectile>().damage = damage;
    newProjectile.GetComponent<Projectile>().trajectory((target.position - transform.position).normalized);
  }




}
