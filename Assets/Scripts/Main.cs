using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
  [Header("Game State Control")]
  public int round;
  public int highScore;
  public bool roundOn;


  [Header("Unit Management")]
  public GameObject gorilla;
  public GameObject[] units;
  private GameObject[] team;
  private GameObject[,] board = new GameObject[16,9];

  [Header("Tiling")]
  public GameObject tileHighlight;
  private GameObject highlight;
  GameObject selectedObject;
  Rigidbody2D selectedBody;
  UnitController selectedController;
    // Start is called before the first frame update
    void Start()
    {
      //spawn a square that highlights tiles.
      highlight = Instantiate(tileHighlight, Vector2.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
      //start round shortcut 
      if(Input.GetKeyDown(KeyCode.Space)){
        StartCoroutine(StartRound());
      } 

      //highlighting tiles between rounds.
      Vector3 mouseScreenPosition = Input.mousePosition;
      Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
      mouseWorldPosition.z = 0f;
      Vector2 mouseWorld = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
     
      //only highlight tiles and allow the dragging of units if the round is not on
      if(!roundOn){
        //math that makes the units stick to the center of the tiles
        highlight.SetActive(true);
        int tileX = (int)Mathf.Floor(mouseWorldPosition.x);
        int tileY = (int)Mathf.Floor(mouseWorldPosition.y+0.5f);
        highlight.transform.position = new Vector2((float)tileX+0.5f, (float)tileY); 

        //if you click and you there is a unit there save the unit temporarily
        if(Input.GetMouseButtonDown(0)){
          Collider2D hit = Physics2D.OverlapPoint(mouseWorld);
          if(hit != null){
            selectedObject = hit.gameObject;
            selectedBody = hit.attachedRigidbody;
          }
        }

        //Stick the clicked unit to the mouse for drag and drop behavior
        if(Input.GetMouseButton(0) && selectedObject){
          selectedBody.MovePosition(mouseWorldPosition);
        }

        //When mouse is released, assign the unit to the new tile and clear what unit is being held
        if(Input.GetMouseButtonUp(0) && selectedObject){
          assignUnitToTile(tileX, tileY, selectedObject);
          selectedObject = null;
          selectedBody = null;
        }

      }

      //check to see if gorillas are all dead
      checkRoundEnd();
    }


  //gorilla spawner 
  void spawnGorilla(){
    GameObject newGorilla = Instantiate(gorilla, getRandomSpawnOutsideMap(Random.Range(1, 5)), Quaternion.identity);
  }
  
  //used with gorilla spawner to spawn a gorilla in a random spot slightly out of view of the camera
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
  
  //Start the round, gorillas will spawn at an interval between 2-5 seconds. The number of gorillas scales with the round.
  IEnumerator StartRound(){
    Debug.Log("Starting round: " + round);
    roundOn = true;
    int gorillaStock = round;
    while(gorillaStock > 0){
      float randomWait = Random.Range(2f, 5f);
      spawnGorilla();
      yield return new WaitForSeconds(randomWait);
      gorillaStock--;
    }
  }

  //if there are no more gorillas on the map the round ends
  void checkRoundEnd(){
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Gorilla");
    if(enemies.Length == 0){
      roundOn = false;
      Debug.Log("Round ended");
    }
  }

  //attempt to assign a unit to a new tile
  void assignUnitToTile(int tileX, int tileY, GameObject selected){
    UnitController selectedController = selected.GetComponent<UnitController>();
    //if a unit doesnt already have that tile claimed,
    if(board[tileX+8,tileY+5] == null){
      //clear the old position
      board[selectedController.oldX(), selectedController.oldY()] = null;
      //claim the new position
      board[tileX+8,tileY+5] = selected;
      //assign the new tile
      selectedController.AssignTile(tileX, tileY);
    }else{
      //if there is already a unit on the tile, go back to your home position.
      selected.transform.position = selectedController.tilePosition;
    }
  }

}


