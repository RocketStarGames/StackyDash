using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndLevelResult : MonoBehaviour
{

    private float TargetCoins;
 private bool UpdateCoins;
    public static int AdsShownCounter = 0;

    public void SetResult()
    {
            //Implementation.Instance.ShowInterstitial();
       
StartCoroutine("SetResultE");
 }
 IEnumerator SetResultE(){
yield return new WaitForSecondsRealtime(1f);
     transform.DOLocalMoveX(0f,0.5f);
    
     transform.GetChild(2).GetComponent<Text>().text ="SCORE : "+((int)Global.CurrentScore).ToString();
     if (Global.CurrentScore>PlayerPrefs.GetInt("BestScore",0)){
         PlayerPrefs.SetInt("BestScore",(int)Global.CurrentScore);
     }
       transform.GetChild(0).GetComponent<Text>().text ="BEST SCORE : " +PlayerPrefs.GetInt("BestScore",0).ToString() ;
transform.SetSiblingIndex(0);
     
     UpdateCoins= true;
      Camera.main.GetComponent<Global>().ScoreHandler.transform.parent.gameObject.transform.DOLocalMoveY(-350f,0.5f);
       Camera.main.GetComponent<Global>().CoinObj.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
  
    


    }

void Update(){

    if (UpdateCoins){
        if ( TargetCoins != PlayerPrefs.GetInt("CoinsSaved",0)){
            TargetCoins = Mathf.MoveTowards(TargetCoins,PlayerPrefs.GetInt("CoinsSaved",0),Time.deltaTime*400f);
            transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = ((int)TargetCoins).ToString();
        }
    }


}
}

