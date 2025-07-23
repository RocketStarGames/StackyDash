using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TapToContinueBehaviour : MonoBehaviour
{

//public GameObject TapToContinueText;
public GameObject TryAgainText;
public float DefaultScaleTA;
public float DefaultScaleTTC;

void OnEnable (){

    DefaultScaleTA = TryAgainText.transform.localScale.x;
 //   DefaultScaleTTC = TapToContinueText.transform.localScale.x;

}

public void AnimateGameOverUi(){
    StartCoroutine("AnimateTA");
     StartCoroutine("AnimateTTC");
}

 IEnumerator AnimateTA(){
    TryAgainText.transform.DOKill();
   TryAgainText.transform.DOScale(DefaultScaleTA*1.2f,1f);
    yield return new WaitForSecondsRealtime(0.99f);
    TryAgainText.transform.DOScale(DefaultScaleTA,0.8f);
     yield return new WaitForSecondsRealtime(0.79f);
    StartCoroutine("AnimateTA");
    
  }

   IEnumerator AnimateTTC(){
  //  TapToContinueText.transform.DOKill();
   // TapToContinueText.transform.DOScale(DefaultScaleTTC*1.2f,0.8f);
    yield return new WaitForSecondsRealtime(0.79f);
    // TapToContinueText.transform.DOScale(DefaultScaleTTC,0.8f);
     yield return new WaitForSecondsRealtime(0.79f);
    StartCoroutine("AnimateTTC");
    
  }
}
