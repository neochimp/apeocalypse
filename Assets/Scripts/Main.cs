using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
  public int round;
  public int highScore;
  public GameObject gorilla;

  public GameObject[] units;
  private GameObject[] team;
  private bool roundOn;
  private int gorillaStock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space)){
        Debug.Log("Spacebar pressed");
        StartCoroutine(StartRound());
      } 
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
  
  IEnumerator StartRound(){
    Debug.Log("Starting round: " + round);
    gorillaStock = round;
    while(gorillaStock > 0){
      float randomWait = Random.Range(2f, 5f);
      spawnGorilla();
      yield return new WaitForSeconds(randomWait);
      gorillaStock--;
    }
  }

}


