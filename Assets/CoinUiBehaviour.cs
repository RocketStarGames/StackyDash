using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CoinUiBehaviour : MonoBehaviour
{
Transform CoinUITarget;
private float ttw;
private GameObject boxposition;
void Awake(){
    CoinUITarget = Camera.main.GetComponent<Global>().CoinObj.transform;
  //  StartCoroutine("Animate");
}

public void AnimateCoin(){
    StartCoroutine("Animate");
}

public void GiveACoinBonus(float TimeToWait,GameObject BoxPos){
    ttw=TimeToWait;
    boxposition= BoxPos;
    StartCoroutine("FinishLineAnimator");
}
IEnumerator FinishLineAnimator(){
  
    yield return new WaitForSecondsRealtime(ttw);
        transform.position= Camera.main.WorldToScreenPoint(boxposition.transform.position);
 transform.parent = Camera.main.GetComponent<Global>().EndGameCanvas.transform;
 PlayerPrefs.SetInt("CoinsSaved",PlayerPrefs.GetInt("CoinsSaved",0)+1);
StartCoroutine("Animate");
}

IEnumerator Animate(){
   
 transform.DOMove(CoinUITarget.position,0.6f);
    transform.DOScale(CoinUITarget.localScale,0.3f);
    yield return new WaitForSecondsRealtime(0.6f);
   CoinUITarget.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("CoinsSaved",0).ToString();
    Destroy(gameObject);
}

}
