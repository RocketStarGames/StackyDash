using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class Global : MonoBehaviour
{
    public GameObject CurrentLvl;
     public GameObject CurrentLvlMainMenu;
    public GameObject ScoreHandler;
    public static float CurrentScore;
    public static  float TargetScore;
    public bool UpdateScore=false;
    public GameObject CoinUi;
    public GameObject warningUi;

    public void animatewarning()
    {
        StartCoroutine("animatewarningui");
    }

    IEnumerator animatewarningui()
    {
        warningUi.GetComponent<Image>().DOKill();
        warningUi.GetComponent<Image>().DOFade(0.2f, 0.3f);
        yield return new WaitForSecondsRealtime(0.29f);
        warningUi.GetComponent<Image>().DOFade(0f, 0.3f);

    }

  
    public GameObject EndLevelResult;
    
    public static bool GameStop=true;

    public GameObject CoinObj;
    public GameObject CoinObjMainMenu;
    
    public GameObject CurrentBoxNumberUi;
private float DefaultCurrentBoxNumberUiSize;
private Color DefaultTextCurrentBoxNumberColor;

public Color TargetColorNumeberIncreement;
public GameObject OurTower;
public GameObject FeverText;

public List<GameObject> Level1Prefabs;
public GameObject FinishLineGo;
public GameObject FinishLine;
public GameObject Trucks;

public int NumberOfLevelPrefabsToSpawn=3;
private int RandomLevel=0;
private int StoredRandomLevel=0;

float SmoothnessBar=0.0f;
float smoothTime =0.3f;


public float DistanceToTravel;
public float DistanceTraveled;
public float DefaultPlayerZPosition;

public GameObject Player;
public Animator PlayerAnimator;


public GameObject LevelProgressionBar;

public GameObject MainMenuCanvas;
public GameObject InGameCanvas;
public GameObject EndGameCanvas;
public GameObject GameOverCanvas;



public void ResetNumber(){
    StartCoroutine("AnimateCurrentBoxNumber");
}



public void TapToContinue(){
    SceneManager.LoadScene("SampleScene");

}

public void Home(){
    SceneManager.LoadScene("Loading");


}

    void OnAwake()
    {

    }


    // Here We Spown Levels till number of level is Zero then spawn FinishLine

    public void SpawnNewLevel(GameObject lvl){
    NumberOfLevelPrefabsToSpawn-=1;
 if (NumberOfLevelPrefabsToSpawn>0){

    while (RandomLevel==StoredRandomLevel){
    RandomLevel = Random.Range(0,Level1Prefabs.Count);
    }
    StoredRandomLevel = RandomLevel;

   GameObject NewLevelPos = Instantiate(Level1Prefabs[RandomLevel],lvl.transform.position,lvl.transform.rotation);
float PosZOffset = (NewLevelPos.transform.GetChild(1).transform.position.z-NewLevelPos.transform.GetChild(0).transform.position.z)/2;
    NewLevelPos.transform.position = new Vector3(NewLevelPos.transform.position.x,NewLevelPos.transform.position.y,lvl.transform.GetChild(1).transform.position.z+PosZOffset);

 }
 else {
 GameObject FinishLineP = Instantiate(FinishLine);
 float PosZOffset = (lvl.transform.GetChild(1).transform.position.z-lvl.transform.GetChild(0).transform.position.z)/2;
    FinishLineP.transform.position = new Vector3(FinishLineP.transform.position.x,FinishLineP.transform.position.y,lvl.transform.GetChild(1).transform.position.z+PosZOffset);
DistanceToTravel = FinishLineP.transform.position.z-Player.transform.position.z;
 }

}





// Here We Ainmate the Box Number Socre 
  IEnumerator AnimateCurrentBoxNumber(){

       CurrentBoxNumberUi.transform.DOKill();
       CurrentBoxNumberUi.GetComponent<Text>().text= (OurTower.GetComponent<TowerController>().boxes.Count).ToString();
        CurrentScore +=1;
        ScoreHandler.GetComponent<Text>().text = CurrentScore.ToString();
      CurrentBoxNumberUi.GetComponent<Text>().DOColor(TargetColorNumeberIncreement,0.2f);
     CurrentBoxNumberUi.transform.DOScale(DefaultCurrentBoxNumberUiSize*1.2f,0.2f);
      yield return new WaitForSecondsRealtime(0.19f);
       CurrentBoxNumberUi.GetComponent<Text>().DOColor(DefaultTextCurrentBoxNumberColor,0.2f);
        CurrentBoxNumberUi.transform.DOScale(DefaultCurrentBoxNumberUiSize,0.1f);
  }



// Some Stuff To Do When The Level Is Completed
IEnumerator LvlCompleted(){
     Debug.Log("end");
    
    return null;
   




}
IEnumerator PutBoxOnTrucks(){
    Debug.Log("Trucks");
    int TrucksLeft = FinishLineGo.GetComponent<InstantiateHeights>().IntNumberOfTrucks;
    int MaxTruckBoxCounter = 0;

    int CurBoxLevels =0;
//Trucks.GetComponent<TruckController>().LevelAmount
    int CurBoxAmountPerRow= 0;
// Trucks.GetComponent<TruckController>().BoxAmountPerRow
    int CurLineAmount = 0;

yield return new WaitForSecondsRealtime(1f);
    for(int i=OurTower.GetComponent<TowerController>().boxes.Count-1;i>=0;i--){


Vector3 TargetPosition =new Vector3(
    Trucks.GetComponent<TruckController>().FBR.transform.localPosition.x-(Trucks.GetComponent<TruckController>().FBR.transform.localScale.x*CurBoxAmountPerRow),
    Trucks.GetComponent<TruckController>().FBR.transform.localPosition.y+(Trucks.GetComponent<TruckController>().FBR.transform.localScale.y*CurBoxLevels),
    Trucks.GetComponent<TruckController>().FBR.transform.localPosition.z-(Trucks.GetComponent<TruckController>().FBR.transform.localScale.z*CurLineAmount)
    );



  
    OurTower.GetComponent<TowerController>().boxes[i].obj.transform.parent = Trucks.transform.GetChild(TrucksLeft-1).transform;
OurTower.GetComponent<TowerController>().boxes[i].obj.transform.DOLocalMove(TargetPosition,0.3f);
OurTower.GetComponent<TowerController>().boxes[i].obj.transform.DOLocalRotateQuaternion(Trucks.GetComponent<TruckController>().FBR.transform.rotation,0.3f);
OurTower.GetComponent<TowerController>().boxes[i].obj.transform.GetChild(0).GetComponent<Renderer>().material.DOColor(Color.white,0.3f);
OurTower.GetComponent<TowerController>().boxes[i].obj.transform.DOScale(Trucks.GetComponent<TruckController>().FBR.transform.localScale.x,0.3f);

CurBoxAmountPerRow+=1;
if( CurBoxAmountPerRow== Trucks.GetComponent<TruckController>().BoxAmountPerRow){
    CurBoxAmountPerRow =0;
    CurBoxLevels+=1;
if (CurBoxLevels==Trucks.GetComponent<TruckController>().LevelAmount){
    CurBoxLevels =0;
    CurLineAmount+=1;

    if(CurLineAmount==Trucks.GetComponent<TruckController>().LineAmount){
        CurBoxLevels=0;
        CurLineAmount = 0;
        CurBoxAmountPerRow = 0;
    }

}

}



MaxTruckBoxCounter+=1;
if(MaxTruckBoxCounter==Trucks.GetComponent<TruckController>().MaxBoxSlotsByTruck){
    TrucksLeft-=1;
   MaxTruckBoxCounter =0;

}
   GameObject CoinUiobj=  Instantiate(CoinUi,Camera.main.WorldToScreenPoint(transform.position),Quaternion.identity);
      CoinUiobj.GetComponent<CoinUiBehaviour>().GiveACoinBonus(0.3f,OurTower.GetComponent<TowerController>().boxes[i].obj.gameObject);

yield return new WaitForSecondsRealtime(0.05f);
    }

    PlayerAnimator.SetInteger("CharAnimStat",0);
 Camera.main.GetComponent<Global>().EndLevelResult.GetComponent<EndLevelResult>().SetResult();
                
}

public void LevelCompleted(GameObject FinishLine){
    FinishLineGo = FinishLine;
FinishLine.GetComponent<InstantiateHeights>().Activatetrucks(OurTower.GetComponent<TowerController>().boxes.Count);
//StartCoroutine("LvlCompleted");
StartCoroutine("PutBoxOnTrucks");
}

// Reset Some Value When We click on the Start Button . Start botton Script
public void StartGame(){
    GetComponent<CameraFollow>().enabled= true;
MainMenuCanvas.SetActive(false);
InGameCanvas.SetActive(true);

        Player.GetComponent<PlayerController>().Zspeed = 26f + (PlayerPrefs.GetInt("CurrentLevel",1)/3f);

        PlayerAnimator.SetInteger("CharAnimStat",1);
PlayerAnimator.SetFloat("RunFactor",Player.GetComponent<PlayerController>().Zspeed/15f);
GameStop=false;
}

    IEnumerator testhaptic()
    {
        yield return new WaitForSecondsRealtime(3f);
      
    }

    void Start(){
        // StartCoroutine("testhaptic");

        if (PlayerPrefs.GetInt("CurrentLevel", 1) >= 1 && PlayerPrefs.GetInt("CurrentLevel", 1) < 5)
        {
            NumberOfLevelPrefabsToSpawn = 6;
        }

        else if (PlayerPrefs.GetInt("CurrentLevel", 1) >= 5 && PlayerPrefs.GetInt("CurrentLevel", 1) < 10)
        {
            NumberOfLevelPrefabsToSpawn = 8;

        }

        else if (PlayerPrefs.GetInt("CurrentLevel", 1) >= 10 && PlayerPrefs.GetInt("CurrentLevel", 1) < 15)
        {
            NumberOfLevelPrefabsToSpawn = 10;

        }

        else if (PlayerPrefs.GetInt("CurrentLevel", 1) >= 15 && PlayerPrefs.GetInt("CurrentLevel", 1) < 20)
        {
            NumberOfLevelPrefabsToSpawn = 11;

        }

        else if (PlayerPrefs.GetInt("CurrentLevel", 1) >= 20 && PlayerPrefs.GetInt("CurrentLevel", 1) < 25)
        {
            NumberOfLevelPrefabsToSpawn = 12;

        }

        else if (PlayerPrefs.GetInt("CurrentLevel", 1) >= 25 && PlayerPrefs.GetInt("CurrentLevel", 1) < 30)
        {
            NumberOfLevelPrefabsToSpawn = 12;

        }

        else if (PlayerPrefs.GetInt("CurrentLevel", 1) >= 30)
        {
            NumberOfLevelPrefabsToSpawn = 15;

        }

        CurrentLvl.GetComponent<Text>().text = PlayerPrefs.GetInt("CurrentLevel",1).ToString();
     CurrentLvlMainMenu.GetComponent<Text>().text = "LEVEL : " + PlayerPrefs.GetInt("CurrentLevel",1).ToString();
     CurrentScore= 0;
PlayerAnimator =  Player.transform.GetChild(4).transform.GetChild(0).GetComponent<Animator>();

CoinObj.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("CoinsSaved",0).ToString();
CoinObjMainMenu.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("CoinsSaved",0).ToString();

DefaultCurrentBoxNumberUiSize =  CurrentBoxNumberUi.transform.localScale.y;
DefaultTextCurrentBoxNumberColor =  CurrentBoxNumberUi.GetComponent<Text>().color;
DefaultPlayerZPosition = Player.transform.position.z;

 }

// Level Progression Bar 
 void LateUpdate(){
   DistanceTraveled =  Player.transform.position.z - DefaultPlayerZPosition;
   
   //LevelProgressionBar.GetComponent<Slider>().value=  DistanceTraveled/DistanceToTravel;
   if (DistanceTraveled/DistanceToTravel>0 && DistanceTraveled/DistanceToTravel<1)
   LevelProgressionBar.GetComponent<Slider>().value = Mathf.MoveTowards(LevelProgressionBar.GetComponent<Slider>().value,DistanceTraveled/DistanceToTravel,Time.deltaTime*0.1f);
  // LevelProgressionBar.GetComponent<Slider>().value= Mathf.SmoothDamp(LevelProgressionBar.GetComponent<Slider>().value,DistanceTraveled/DistanceToTravel,ref SmoothnessBar,smoothTime);

if (UpdateScore){
    Debug.Log("UpdateScore");
    if (TargetScore!=CurrentScore){
     CurrentScore = Mathf.MoveTowards(CurrentScore,TargetScore,Time.deltaTime*TargetScore);
         ScoreHandler.GetComponent<Text>().text = ((int)CurrentScore).ToString();
    }

    else
    {
        UpdateScore = false;
    }
}
 }
}
