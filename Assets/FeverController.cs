using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FeverController : MonoBehaviour
{
     
     public int FeverCount=0;
    public GameObject NumberText;
public float DefaultScale;


private bool ResetToZeroFail;
private bool ResetToZero;
private float SmoothnessBar=0.001f;
private float smoothTime=0.2f;
private float smoothTimeFail=2f;

public GameObject TowerController;

//TargetValue
public float TopDragFever;
public float TopForceScaleFever;
public float TopWindScaleFever;
public float TopWindSpeedFever;
public float TopBalanceLossFever;

public float DragFever;
public float ForceScaleFever;
public float ZswayFever;
public float StifnessFever;
public float WobbleFever;
public float DifficultyWindScaleFever;
public float DifficulyWindSpeedFever;


// Stored Value
private float TopDragStored;
private float TopForceScaleStored;
private float TopWindScaleStored;
private float TopWindSpeedStored;
private float TopBalanceLossStored;

private float DragStored;
private float ForceScaleStored;
private float ZswayStored;
private float StifnessStored;
private float WobbleStored;
private float DifficultyWindScaleStored;
private float DifficulyWindSpeedStored;

private bool GameOver=false;

public GameObject FeverSlider;
    public GameObject FeverBackground;
    public GameObject perfectmessage;

void Start(){

    //  DefaultGlobalScale=  transform.localScale.x;
}
    IEnumerator waitmoment()
    {
       
        TowerController.GetComponent<TowerController>().FeverEnabled = true;
        yield return new WaitForSecondsRealtime(0.2f);
      // provisional
       // StoringBalanceValues();
       // ChangeBalanceValues();

     
    }

    public void IncrementFever(){
if (GameOver==false){
     if (ResetToZero==false && ResetToZeroFail==false){
      FeverCount+=1;
      FeverSlider.GetComponent<Slider>().value +=0.05f;

      if (FeverSlider.GetComponent<Slider>().value >=1){
                    perfectmessage.GetComponent<PerfectmESSAGE>().ShowBonusMessage();
                    Global.CurrentScore += 100;
                   Camera.main.GetComponent<Global>().ScoreHandler.GetComponent<Text>().text = Global.CurrentScore.ToString();
                    StartCoroutine("waitmoment");
                    StartCoroutine("FeverModeActive");
                    StartCoroutine("backgroundFever");
                    ResetToZero = true;
                }

 StartCoroutine("FeverVFX");
     }
}
  }

  IEnumerator FeverModeActive(){
     transform.DOKill();
    transform.DOScale(DefaultScale*1.4f,0.3f);
       
    yield return new WaitForSecondsRealtime(0.29f);
     transform.DOScale(DefaultScale,0.25f);
     yield return new WaitForSecondsRealtime(0.24f);
    StartCoroutine("FeverModeActive");
    
  }
    IEnumerator backgroundFever()
    {
        FeverBackground.GetComponent<Image>().DOFade(0.25f, 0.3f);
        yield return new WaitForSecondsRealtime(0.29f);
        FeverBackground.GetComponent<Image>().DOFade(0.1f, 0.26f);
        yield return new WaitForSecondsRealtime(0.26f);
        StartCoroutine("backgroundFever");
    }

    public void CancelFeverGameOver(){
    StopCoroutine("FeverModeActive");
        StopCoroutine("backgroundFever");
      
        ResetToZero =false;
    GameOver=true;
    ResetToZeroFail = true;
    transform.DOScale(0f,0.3f);
        FeverBackground.GetComponent<Image>().DOKill();
        FeverBackground.GetComponent<Image>().DOFade(0f, 0.2f);

    }

    void Update(){
    if (ResetToZero ==true){
  //FeverSlider.GetComponent<Slider>().value = Mathf.SmoothDamp(FeverSlider.GetComponent<Slider>().value,0f,ref SmoothnessBar,smoothTime);
  FeverSlider.GetComponent<Slider>().value = Mathf.MoveTowards(FeverSlider.GetComponent<Slider>().value ,0f,Time.deltaTime*smoothTime);
  if (FeverSlider.GetComponent<Slider>().value ==0f){
   // ResetBalanceValue();
     transform.DOKill();
     StopCoroutine("FeverModeActive");
                StopCoroutine("backgroundFever");
                FeverBackground.GetComponent<Image>().DOKill();
                FeverBackground.GetComponent<Image>().DOFade(0f, 0.2f);
                transform.DOScale(0f,0.25f);
    
     TowerController.GetComponent<TowerController>().FeverEnabled=false;
    ResetToZero =false;
  }
    }

    if (ResetToZeroFail ==true){
   FeverSlider.GetComponent<Slider>().value = Mathf.MoveTowards(FeverSlider.GetComponent<Slider>().value ,0f,Time.deltaTime*smoothTimeFail);
     if (FeverSlider.GetComponent<Slider>().value ==0f){
    ResetToZeroFail =false;
  }
    }
  }



IEnumerator FeverVFX(){
return null;

}

  public void ResetFever(){
    if (ResetToZero==false && ResetToZeroFail==false){
    ResetToZeroFail =true;
      FeverCount=  0;
      NumberText.transform.DOKill();
      transform.DOKill();
      transform.DOScale(0,0.5f);
    }
  }



public void StoringBalanceValues(){
  // Storing Previous Value
   TopDragStored = TowerController.GetComponent<TowerController>().topDrag;
TopForceScaleStored = TowerController.GetComponent<TowerController>().topForceScale ;
 TopWindScaleStored =  TowerController.GetComponent<TowerController>().topWindScale;
 TopWindSpeedStored =  TowerController.GetComponent<TowerController>().topWindSpeed;
 TopBalanceLossStored =  TowerController.GetComponent<TowerController>().topBalanceLoss;
 DragStored=  TowerController.GetComponent<TowerController>().drag;
 ForceScaleStored = TowerController.GetComponent<TowerController>().forceScale;
 ZswayStored =  TowerController.GetComponent<TowerController>().zSway;
 StifnessStored =  TowerController.GetComponent<TowerController>().stiffness;
 WobbleStored = TowerController.GetComponent<TowerController>().wobble;
 DifficultyWindScaleStored = TowerController.GetComponent<TowerController>().difficultyWindScale;
 DifficulyWindSpeedStored =  TowerController.GetComponent<TowerController>().difficultyWindSpeed;
}
public void ChangeBalanceValues(){
  
TowerController.GetComponent<TowerController>().topDrag=TopDragFever ;
TowerController.GetComponent<TowerController>().topForceScale = TopForceScaleFever;
TowerController.GetComponent<TowerController>().topWindScale= TopWindScaleFever;
TowerController.GetComponent<TowerController>().topWindSpeed= TopWindSpeedFever;
TowerController.GetComponent<TowerController>().topBalanceLoss= TopBalanceLossFever;
TowerController.GetComponent<TowerController>().drag= DragFever;
TowerController.GetComponent<TowerController>().forceScale= ForceScaleFever;
TowerController.GetComponent<TowerController>().zSway= ZswayFever;
TowerController.GetComponent<TowerController>().stiffness= StifnessFever;
TowerController.GetComponent<TowerController>().wobble= WobbleFever;
TowerController.GetComponent<TowerController>().difficultyWindScale= DifficultyWindScaleFever;
TowerController.GetComponent<TowerController>().difficultyWindSpeed= DifficulyWindSpeedFever;
}

public void ResetBalanceValue(){
  TowerController.GetComponent<TowerController>().topDrag= TopDragStored;
TowerController.GetComponent<TowerController>().topForceScale = TopForceScaleStored;
TowerController.GetComponent<TowerController>().topWindScale= TopWindScaleStored;
TowerController.GetComponent<TowerController>().topWindSpeed= TopWindSpeedStored;
TowerController.GetComponent<TowerController>().topBalanceLoss= TopBalanceLossStored;
TowerController.GetComponent<TowerController>().drag= DragStored;
TowerController.GetComponent<TowerController>().forceScale= ForceScaleStored;
TowerController.GetComponent<TowerController>().zSway= ZswayStored;
TowerController.GetComponent<TowerController>().stiffness= StifnessStored;
TowerController.GetComponent<TowerController>().wobble= WobbleStored;
TowerController.GetComponent<TowerController>().difficultyWindScale= DifficultyWindScaleStored;
TowerController.GetComponent<TowerController>().difficultyWindSpeed= DifficulyWindSpeedStored;
}




}
