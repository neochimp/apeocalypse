using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
  public GameObject titleText;
  public GameObject backgroundLightning;
  public GameObject whiteness;
  private AudioSource sfx;
  public AudioClip thunder;
    // Start is called before the first frame update
    void Start()
    {
        sfx = gameObject.GetComponent<AudioSource>();
        backgroundLightning.SetActive(false);
        whiteness.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space)){
         StartCoroutine(Lightning());
         StartCoroutine(blinkingText());
       } 
    }

    IEnumerator Lightning(){
      sfx.PlayOneShot(thunder);
      backgroundLightning.SetActive(true);
      yield return new WaitForSeconds(0.1f);
      whiteness.SetActive(true);
      yield return new WaitForSeconds(0.05f);
      backgroundLightning.SetActive(false);
      whiteness.SetActive(false);
    }
    IEnumerator blinkingText(){
      yield return new WaitForSeconds(0.2f);
      titleText.SetActive(false);
      yield return new WaitForSeconds(0.2f);
      titleText.SetActive(true);
      yield return new WaitForSeconds(0.2f);
      titleText.SetActive(false);
      yield return new WaitForSeconds(0.2f);
      titleText.SetActive(true);
      yield return new WaitForSeconds(0.2f);
      titleText.SetActive(false);
      yield return new WaitForSeconds(0.2f);
      titleText.SetActive(true);
      yield return new WaitForSeconds(0.2f);
      titleText.SetActive(false);
      yield return new WaitForSeconds(0.2f);
      titleText.SetActive(true);
      SceneManager.LoadScene("GameplayScene"); 
    }



}
