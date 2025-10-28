using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterButton : MonoBehaviour
{
    public bool selected;
    public int id;
    private CharacterSelect selectMenu;
    private Image img;
    private Image sprite;
    private Main main;
    public bool unlocked;
    public int cost;
    private TextMeshProUGUI purchaseCost;
    // Start is called before the first frame update
    void Start()
    {
      selectMenu = GetComponentInParent<CharacterSelect>();
      img = GetComponentInChildren<Image>();
      sprite = transform.GetChild(0).GetComponent<Image>();
      main = GameObject.Find("Main Camera").GetComponent<Main>();
      purchaseCost = GetComponentInChildren<TextMeshProUGUI>();
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
        purchaseCost.text = $"{cost}";
      }else{
        sprite.color = Color.white;
        purchaseCost.text = "";
      }
    }

    public void onButtonClick(){
      if(!unlocked){
        attemptPurchase();
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

    public void attemptPurchase(){
      if(main.coins < cost){
        return;
      }else{
        main.SubCoins(cost);
        unlocked = true;
      }
    }
}
