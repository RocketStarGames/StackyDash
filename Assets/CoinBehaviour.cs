using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
 public GameObject CoinUi;
 private bool done=  false;
 void OnTriggerEnter(Collider Col){
     if (Col.gameObject.tag=="Player" || Col.gameObject.tag=="Box" ){
         if ( done== false){
             done = true;
                Debug.Log("CoinCollected");
      GameObject CoinUiobj=  Instantiate(CoinUi,Camera.main.WorldToScreenPoint(transform.position),Quaternion.identity);
      CoinUiobj.transform.parent = Camera.main.GetComponent<Global>().InGameCanvas.transform;
      CoinUiobj.GetComponent<CoinUiBehaviour>().AnimateCoin();
      PlayerPrefs.SetInt("CoinsSaved",PlayerPrefs.GetInt("CoinsSaved",0)+1);
      Destroy(gameObject);
         }

     }
 }

    private void Update()
    {
        transform.Rotate(Vector3.up, 40f * Time.deltaTime);
    }
}
