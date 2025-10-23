using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
  public int round;
  public int highScore;
  public GameObject gorilla;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //spawnGorilla();
    }


  void spawnGorilla(){
    GameObject newGorilla = Instantiate(gorilla, getRandomSpawnOutsideMap(Random.Range(1, 5)), Quaternion.identity);
  }
  
  Vector2 getRandomSpawnOutsideMap(int side){
    float randomX, randomY;
    switch(side){
      case 1: //up
        randomX = Random.Range(-10f, 10f);
        return new Vector2(randomX, 7);
      case 2: //down
        randomX = Random.Range(-10f, 10f);
        return new Vector2(randomX, -6);
      case 3: //left
        randomY = Random.Range(-5f, 5f);
        return new Vector2(-12, randomY);
      case 4: //right
        randomY = Random.Range(-5f, 5f);
        return new Vector2(12, randomY);
      default:
        return Vector2.zero;
    }
    
  
  }
}
