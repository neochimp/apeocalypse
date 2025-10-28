using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public bool menuActive;
    public Main main;
    private HashSet<int> activeTeam;
    // Start is called before the first frame update
    void Start()
    {
      //this.gameObject.SetActive(false);
      //menuActive = false;

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void StartCharacterSelect(HashSet<int> team){
      menuActive = true;
      activeTeam = team;
      this.gameObject.SetActive(true);
    }

    public int toggleFromTeam(int id){
     if(activeTeam.Contains(id)){
       activeTeam.Remove(id);
     }else{
       activeTeam.Add(id);
     }
     return 0;
    }

    public void EndCharacterSelect(){
      menuActive = false;
      this.gameObject.SetActive(false);
      main.StartGame();
    }
    
}
