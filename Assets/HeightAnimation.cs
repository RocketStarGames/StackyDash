using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class HeightAnimation : MonoBehaviour
{
public Color ColorA;
Vector3  DefaultScale;
private  bool done=false;
public bool Reached=false;
void Start(){

    DefaultScale = transform.GetChild(0).transform.localScale;
}
 IEnumerator Anim(){
     Reached=true;
transform.GetChild(0).transform.DOScale(DefaultScale*1.2f,0.3f);
transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>().DOColor(ColorA,0.2f);
yield return new WaitForSecondsRealtime(0.3f);
transform.GetChild(0).transform.DOScale(DefaultScale,0.15f);

 }

 // Animate a reached Height Text
    void OnTriggerEnter (Collider Col){
        if (Col.gameObject.tag  == "Box" && done==false)
        {
            done= true;
        StartCoroutine("Anim");
        }
    }
}
