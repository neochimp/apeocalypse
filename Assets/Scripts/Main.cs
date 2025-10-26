using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
  public int round;
  public int highScore;
  public GameObject gorilla;

  public GameObject[] units;
  private GameObject[] team;
  public GameObject tileHighlight;
  public bool roundOn;
  private int gorillaStock;
  private GameObject highlight;
  private GameObject[,] board = new GameObject[16,9];
  public LayerMask draggableMask = ~3;
  public Transform selectedObject;
  public Rigidbody2D selectedBody;
  public UnitController selectedController;
    // Start is called before the first frame update
    void Start()
    {
        highlight = Instantiate(tileHighlight, Vector2.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space)){
        Debug.Log("Spacebar pressed");
        StartCoroutine(StartRound());
      } 

      //highlighting tiles between rounds.
      Vector3 mouseScreenPosition = Input.mousePosition;
      Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
      mouseWorldPosition.z = 0f;
      Vector2 mouseWorld = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
      
      if(!roundOn){
        highlight.SetActive(true);
        int tileX = (int)Mathf.Floor(mouseWorldPosition.x);
        int tileY = (int)Mathf.Floor(mouseWorldPosition.y+0.5f);
        highlight.transform.position = new Vector2((float)tileX+0.5f, (float)tileY); 

        if(Input.GetMouseButtonDown(0)){
          Collider2D hit = Physics2D.OverlapPoint(mouseWorld);
          if(hit != null){
            selectedObject = hit.transform;
            selectedBody = hit.attachedRigidbody;
            selectedController = hit.GetComponent<UnitController>();
          }
        }

        if(Input.GetMouseButton(0) && selectedObject){
          selectedBody.MovePosition(mouseWorldPosition);
        }

        if(Input.GetMouseButtonUp(0) && selectedObject){
          if(board[tileX+8,tileY+5] == null){
            board[selectedController.oldX(), selectedController.oldY()] = null;
            selectedController.AssignTile(tileX, tileY);
          }
          selectedObject = null;
          selectedBody = null;
          selectedController = null;
        }

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
    roundOn = true;
    gorillaStock = round;
    while(gorillaStock > 0){
      float randomWait = Random.Range(2f, 5f);
      spawnGorilla();
      yield return new WaitForSeconds(randomWait);
      gorillaStock--;
    }
    roundOn = false;
  }

}


