using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimateTapToContinue : MonoBehaviour
{
   private float DefaultS;
    void Start()
    {
        DefaultS = transform.localScale.x;
        
        StartCoroutine("Animate");
    }

  
  IEnumerator  Animate(){
transform.DOScale(DefaultS*1.1f,0.8f);
yield return new  WaitForSecondsRealtime(0.79f);
transform.DOScale(DefaultS,0.6f);
yield return new  WaitForSecondsRealtime(0.59f);
StartCoroutine("Animate");
  }
}
