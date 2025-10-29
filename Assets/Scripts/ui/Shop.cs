using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public Main main;
    public Transform shopkeep;
    public TextMeshProUGUI shopText;
    public TextMeshProUGUI healCost;
    public TextMeshProUGUI dmgCost;
    public TextMeshProUGUI rezCost;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        shopkeep.position = new Vector2(2620f, 400f);
        shopText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OpenShop(){
      this.gameObject.SetActive(true);
      StartCoroutine(OpenShopRoutine()); 
    }

    public void CloseShop(){
      this.gameObject.SetActive(false);
    }
    
    private IEnumerator OpenShopRoutine(){
      var rb = shopkeep.GetComponent<Rigidbody2D>();
      rb.velocity = new Vector2(-300f, 0f);

      while(shopkeep.position.x > 1960f){
        yield return null;
      }

      rb.velocity = Vector2.zero;
      shopText.text = "Welcome shop... Gorillas? Don't care... Me Want Banana.";
    }

    public void healPotion(){
      if(main.coins >= 1){
        main.SubCoins(1);
        GameObject[] units = GameObject.FindGameObjectsWithTag("Human");
        foreach(GameObject unit in units){
          unit.GetComponent<UnitController>().HealDamage(-1);
        }
      }
    }
    public void damagePotion(){
      if(main.coins >= 2){
        main.SubCoins(2);
        GameObject[]units = GameObject.FindGameObjectsWithTag("Human");
        foreach(GameObject unit in units){
          //implement damage up.
        }
      }
    }
}
