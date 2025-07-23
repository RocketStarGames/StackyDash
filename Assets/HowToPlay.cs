using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HowToPlay : MonoBehaviour
{

public float leftTarget;
public float RightTarget;

void Start(){
    StartCoroutine("anim");


}
IEnumerator anim(){
    transform.DOLocalMoveX(leftTarget,1.3f);
    yield return new WaitForSecondsRealtime(1.28f);
    transform.DOLocalMoveX(RightTarget,1.3f);
        yield return new WaitForSecondsRealtime(1.28f);
         StartCoroutine("anim");
}

public void KillHTP(){
    GetComponent<SpriteRenderer>().DOFade(0f,0.5f);
     transform.parent.GetComponent<SpriteRenderer>().DOFade(0f,0.5f);


}
}
