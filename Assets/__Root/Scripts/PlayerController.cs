using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

    //Slide Movement Variables.***************
    [Range(0, 1)] public float smoothness = 0.5f;
    [Range(0, 10)] public float SensivityFactor = 2f;
    private bool _drag = false;
    private float input = 0;
    private float _inputOffset = 0;
    public Vector3 DefaultBoxSize;
    //public Vector3 Dvelocity;
    public float Xspeed = 0;
    public float Zspeed = 10;
    //End Of Slide Movement Variables.*****************

    Camera MyCam;
    private Rigidbody _rb;
    GameObject TowerController;
    private bool LevelEnded=false;
    public Animator PlayerAnimator;


    public int HeightRange;


    private GameObject FinishLine;
    public int MaxHeighReached;

    public GameObject OurHeightReached;
    public Rigidbody RB {
        get {
            if (_rb == null)
                _rb = GetComponent<Rigidbody>();
            return _rb;
        }
    }


    void Start() {
        TowerController = transform.GetChild(0).gameObject;
        MyCam = Camera.main;
        
    }

    IEnumerator DoAfter(){

        //Here We hide All Levels heights . and we Get the Highest Point we reached . by checking on the heights list from the final point . 
        yield return new WaitForSecondsRealtime(0.6f);
              for (int i=FinishLine.transform.childCount-1; i>3;i--){
             FinishLine.transform.GetChild(i).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().DOFade(0f,0.3f);

         }
          FinishLine.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().DOFade(0f,0.3f);
        yield return new WaitForSecondsRealtime(0.4f);
        for (int i=FinishLine.transform.childCount-1; i>3;i--){
            if(FinishLine.transform.GetChild(i).GetComponent<HeightAnimation>().Reached==true){

   MaxHeighReached = ((i-2)-1)*5;
    Camera.main.GetComponent<Global>().ScoreHandler.transform.GetChild(1).GetComponent<Text>().text = "SCORE X" +MaxHeighReached.ToString();
  Global.TargetScore =  Global.CurrentScore*MaxHeighReached;
   // Creating the Animation of which level We Reached 
if(MaxHeighReached>PlayerPrefs.GetInt("BestHeightReached",0)){
    PlayerPrefs.SetInt("BestHeightReached",MaxHeighReached);
       OurHeightReached.GetComponent<HeightTextAnim>().GoldenColor();
}
   OurHeightReached.transform.GetChild(1).GetComponent<Text>().text= "BEST:"+ PlayerPrefs.GetInt("BestHeightReached",0).ToString()+"M";
   
   OurHeightReached.GetComponent<HeightTextAnim>().Anim(MaxHeighReached,0.8f);
    
break;
   
            }
      

        }
        if (MaxHeighReached==0){
              Camera.main.GetComponent<Global>().ScoreHandler.transform.GetChild(1).GetComponent<Text>().text = "" ;
                    OurHeightReached.transform.GetChild(1).GetComponent<Text>().text= "BEST:"+ PlayerPrefs.GetInt("BestHeightReached",0).ToString()+"M";
    
   OurHeightReached.GetComponent<HeightTextAnim>().Anim(MaxHeighReached,0f);
        }

    }


//// Here Start Triggers For level End. 
    void OnTriggerEnter(Collider Col){
        
        if(Col.gameObject.tag=="FinishLine" && LevelEnded==false){
            transform.GetComponent<Collider>().enabled = false;
            transform.GetChild(0).GetComponent<Collider>().enabled = false;
            LevelEnded = true;
            Debug.Log("PlayerArrivedToFinishLine");
FinishLine=  Col.gameObject;
StartCoroutine("DoAfter");

            Global.GameStop=true;
            TowerController.GetComponent<TowerController>().FreezeBoxes();
            
FinishLine.GetComponent<InstantiateHeights>().MoveBarier();
           Camera.main.GetComponent<Global>().EndGameCanvas.SetActive(true);
             Camera.main.GetComponent<Global>().ScoreHandler.transform.parent.transform.parent =  Camera.main.GetComponent<Global>().EndGameCanvas.transform;
                Camera.main.GetComponent<Global>().CoinObj.transform.parent =  Camera.main.GetComponent<Global>().EndGameCanvas.transform;
           
            Camera.main.GetComponent<Global>().InGameCanvas.SetActive(false);
            Camera.main.GetComponent<Global>().LevelProgressionBar.SetActive(false);
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 1) + 1);

        }
    }

    public bool isCollision;
    public Vector3 CollisionForce =Vector3.zero;
    private void OnCollisionEnter(Collision collision){
        isCollision =true;
        CollisionForce = collision.impulse;
    }

    Vector3 lastPosition = Vector3.zero;

    private void Update() {

       
        //Slide Control *******************************************************
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            _drag = false;
        }

        //Start
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            _drag = true;
            _inputOffset = 0;
        }
    }

    void FixedUpdate() {
 if(Global.GameStop==false){
        //Get X Axis Speed
        Vector3 curPos = new Vector3(transform.position.x, 0f, 0f);
        
        Xspeed = Mathf.Abs((curPos - lastPosition).magnitude / Time.fixedDeltaTime);
        // CheckDirection
        if (transform.position.x < lastPosition.x)
            Xspeed = Xspeed * -1f;
        
        // Get Last Frame 
        lastPosition = new Vector3(transform.position.x, 0f, 0f);
        if (TowerController.GetComponent<TowerController>().boxes.Count!=0){
        if(Xspeed>5){
PlayerAnimator.SetInteger("CharAnimStat",2);
        }

        else if (Xspeed<-5){
PlayerAnimator.SetInteger("CharAnimStat",3);
        }
        }

        //MovingForward

        //Move
        if (_drag) {

            //Distance to the ball.
            float dist = MyCam.transform.InverseTransformPoint(transform.position).z;

            //Offset to subtract from scaled input
            float halfScreenScale = (Screen.width * SensivityFactor - Screen.width) / SensivityFactor;

            //Input scaled and recentered
            Vector3 scaledInput = new Vector3(Input.mousePosition.x * SensivityFactor - halfScreenScale, 0, dist);

            //Input converted to world space
            Vector3 targetPosition = MyCam.ScreenToWorldPoint(scaledInput);
            if (_inputOffset == 0)
                _inputOffset = transform.position.x - targetPosition.x;

            //We will use the x component to move the  ball.
            input = Mathf.Lerp(transform.position.x, targetPosition.x + _inputOffset, smoothness);
        }
        else {
            input = transform.position.x;
        }

        float xPos = Mathf.Clamp(input, -100f, 100f);
        //transform.position = new Vector3(xPos, transform.position.y, transform.position.z + Time.fixedDeltaTime * Zspeed);

        RB.linearVelocity = new Vector3((xPos - transform.position.x)/Time.fixedDeltaTime, 0, Zspeed);

        //End of slide control ********************************************


    }
    if (LevelEnded==true){
    Zspeed = Mathf.Lerp(Zspeed,0f,1*Time.fixedDeltaTime*3f);
     RB.linearVelocity = new Vector3(0, 0, Zspeed);
     if (TowerController.GetComponent<TowerController>().boxes.Count!=0){
     PlayerAnimator.SetInteger("CharAnimStat",4);
     }
     else{
         PlayerAnimator.SetInteger("CharAnimStat",0);
     }
     if(Zspeed>1f){
     PlayerAnimator.SetFloat("RunFactor",Mathf.Lerp(PlayerAnimator.GetFloat("RunFactor"),0f,1*Time.fixedDeltaTime*3f));
     }
     else{

   Zspeed=0;
   RB.linearVelocity = new Vector3(0, 0, Zspeed);
   RB.isKinematic =true;
   LevelEnded=false;
   Camera.main.GetComponent<Global>().LevelCompleted(FinishLine);
     }

}
    }

    //Stop

}
