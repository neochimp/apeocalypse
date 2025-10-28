using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] frames;
    private int currentFrame;
    public float frameRate = 10f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
      if(frames.Length == 0) return;
      timer += Time.deltaTime;
      if(timer >= 1f / frameRate){
        timer -= 1f / frameRate;
        currentFrame = (currentFrame + 1) % frames.Length;
        spriteRenderer.sprite = frames[currentFrame];
      }
    }
 
    public void SpawnCoin(Vector2 spawn){
      Debug.Log("dropping coin");
      GameObject newCoin = Instantiate(this.gameObject, spawn, Quaternion.identity);
      return;
    }

    public void PickupCoin(){

      Destroy(this.gameObject);
    }
}
