using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public bool selected;
    public int id;
    private CharacterSelect selectMenu;
    private Image img;
    private Image sprite;
    public bool unlocked;
    public int cost;
    // Start is called before the first frame update
    void Start()
    {
      selectMenu = GetComponentInParent<CharacterSelect>();
      img = GetComponentInChildren<Image>();
      sprite = transform.GetChild(0).GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
      if(selected){
        img.color = Color.green;
      }else{
        img.color = Color.white;
      }

      if(!unlocked){
        sprite.color = Color.black;
      }else{
        sprite.color = Color.white;
      }
    }

    public void onButtonClick(){
      if(!unlocked){
        return;
      }
      selectMenu.toggleFromTeam(id);
      if(selected){
        selected = false;
      }else{
        selected = true;
      }
      return;
    }
}
