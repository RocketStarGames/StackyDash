using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ayoub;
using AyoubInterpolaters;
using System;

public class TowerController : MonoBehaviour {
    public bool FeverEnabled=false;

    public Color StableColor;
    public Color InstableColor;
    public Color FeverColor;

    public int score = 0;
    public Text scoreText;
    public Material FeverTextMat;
    public Transform character;
    public GameObject firstRefBox;
    public GameObject SecondRefBox;

    public static int AdsShownCounter;

    [HideInInspector]
    public Vector3 boxOffset, towerOffset = new Vector3(0, 1, 0);

    public int NumberOfBox { get { return boxes.Count; } }


    [Header("Tower Props:")]
    [Tooltip("Should be greater than sway, recommend 0.5 or higher.")]
    [Range(0.001f, 1)] public float topDrag = 0.075f;

    [Tooltip("Should be less than drag")]
    [Range(0.001f, 1)] public float topForceScale = 0.075f;

    [Tooltip("The strength of the wind force.")]
    [Range(0f, 2f)] public float topWindScale = 0.8f;
    [Tooltip("The strength of the wind force.")]
    [Range(0f, 2f)] public float topWindSpeed = 0.8f;
    [Tooltip("The strength of the wind force.")]
    [Range(0f, 1f)] public float topBalanceLoss = 0.5f;

    [Header("Block Props:")]
    [Tooltip("Should be greater than sway, recommend 0.5 or higher.")]
    [Range(0.001f, 1)] public float drag = 0.075f;

    [Tooltip("Should be less than drag")]
    [Range(0.001f, 1)] public float forceScale = 0.075f;

    [Tooltip("How much bend there is in the Z axis.")]
    [Range(0.001f, 8)] public float zSway = 0.075f;

    [Tooltip("0, block is pulled toward the base to 1, block is pulled to previous block.")]
    [Range(0f, 1)] public float stiffness = 0.5f;

    [Tooltip("How much next-frame prediction can miss by, higher values allow more wobble but can be unstable.")]
    [Range(0f, 1)] public float wobble = 0.1f;

    [Tooltip("The strength of the wind force.")]
    [Range(0f, 2f)] public float difficultyWindScale = 0.8f;
    
    [Tooltip("The frequency of the wind force.")]
    [Range(0.2f, 2f)] public float difficultyWindSpeed = 0.5f;

   
    //public float breakAngleBase = 1;
    //public float breakAngleTop = 2;
   

         [Tooltip("force With Collision Greater than this will break the tower")]
     [Range(0,20)] public float BreakDistanceZ;

    public AnimationCurve breakAngles = new AnimationCurve(new Keyframe(0, 15), new Keyframe(50, 1));

    [Header("Debug:")]
    public bool debugDrawBreakAngles = false;
    public GameObject debugBoxPrefab;

    private PlayerController _controller;
    public PlayerController Controller {
        get {
            if (_controller == null)
                _controller = transform.parent.GetComponent<PlayerController>();
            return _controller;
        }
    }

    //A list to hold all the boxes in the tower.
    [HideInInspector]
    public List<Box> boxes = new List<Box>();
    
    void Start() {
        //Set initial the position and spacing of the bricks.
        boxOffset = SecondRefBox.transform.position - firstRefBox.transform.position;
        towerOffset = firstRefBox.transform.localPosition;

        Destroy(firstRefBox);
        Destroy(SecondRefBox);

        boxes.Clear();
        //Implementation.Instance?.ShawBanner();
    }

    //A timer for updating the score
    private float _scoreTick = 1;
    private float ScoreTick {
        get { return _scoreTick; }
        set { _scoreTick = value < 0 ? value + 1 : value; }
    }

    //Set the score text
    private void UpdateScore() {
        scoreText.text = string.Format("- {0} -", score);
    }

    private void Update() {

        if (Global.GameStop==false){
        //if the score timer is at zero, add to the score.
        if (ScoreTick < Time.deltaTime) {
            score += NumberOfBox;
            UpdateScore();
        }

        if (Input.GetKey(KeyCode.Space)) {
            Instantiate(debugBoxPrefab, transform.position, Quaternion.identity);
        }

        //Run the timer
        ScoreTick -= Time.deltaTime;
        
        if (debugDrawBreakAngles) {
            Vector3 pointA = transform.position;
            Vector3 pointB = transform.position;
            for (int i = 0; i < NumberOfBox; i++) {
                float angle = breakAngles.Evaluate(i);

                Quaternion rot = Quaternion.Euler(0, 0, angle);
                Vector3 point = Vector3.up * boxes[i].yDefault;
                point = rot * point;

                Debug.DrawLine(transform.position + point, pointA, Color.blue);
                pointA = transform.position + point;

                point.Scale(new Vector3(-1, 1, 1));

                Debug.DrawLine(transform.position + point, pointB, Color.blue);
                pointB = transform.position + point;
            }
        }
// Make the character Follow First Box X Position
        if (NumberOfBox > 0 && boxes[0].update)
            character.position =new Vector3(boxes[0].obj.position.x - towerOffset.x,character.position.y,character.position.z);
        }
    }

    [HideInInspector]
    public Vector3 topPosition = Vector3.zero;
    private Vector3 topVel = Vector3.zero;

    public void SendBoxesToTruckSlots(){
   for (int i=0 ; i<boxes.Count;i++){

            boxes[i].obj.transform.parent = gameObject.transform;
            boxes[i].obj.GetComponent<Rigidbody>().isKinematic= true;
        }

    }

    public void FreezeBoxes(){
        for (int i=0 ; i<boxes.Count;i++){

            boxes[i].obj.transform.parent = gameObject.transform;
            boxes[i].obj.GetComponent<Rigidbody>().isKinematic= true;
        }
    }
    // Freezing Boxes In GameOver
        public void FreezeBoxesGameOver(){
        for (int i=0 ; i<boxes.Count;i++){
            boxes[i].obj.transform.parent = gameObject.transform;
            boxes[i].obj.GetComponent<Rigidbody>().isKinematic= false;
                boxes[i].obj.GetComponent<BoxCollider>().material = null;
                Controller.RB.linearVelocity = new Vector3(0, 0, 0);
                 Controller.RB.isKinematic = true;
                boxes[i].obj.GetComponent<Rigidbody>().useGravity= true;
            boxes[i].obj.gameObject.layer =Layers.OffTower;
        }
    }
    IEnumerator SwitchAnimationToPlayerDown(){
yield return new  WaitForSecondsRealtime(0.5f);
 Controller.PlayerAnimator.SetInteger("CharAnimStat",6);

    }

    private void FixedUpdate() {

        if (Controller.isCollision){
            Controller.isCollision=false;
            Debug.Log($"{Controller.CollisionForce.ToString("0.0000000")}");
                if(Controller.CollisionForce.z <-4){
                Global.GameStop=true;
                //Implementation.Instance?.ShowInterstitial();

                Controller.PlayerAnimator.SetInteger("CharAnimStat",5);
 Controller.RB.linearVelocity = new Vector3(0, 0,0);
            Camera.main.GetComponent<Global>().FeverText.GetComponent<FeverController>().CancelFeverGameOver();
            Camera.main.GetComponent<Global>().GameOverCanvas.SetActive(true);
            Camera.main.GetComponent<Global>().InGameCanvas.SetActive(false);
            Camera.main.GetComponent<Global>().LevelProgressionBar.SetActive(false);
 //            Camera.main.GetComponent<Global>().GameOverCanvas.transform.GetChild(1).GetComponent<TapToContinueBehaviour>().AnimateGameOverUi();

            StartCoroutine("SwitchAnimationToPlayerDown");
            FreezeBoxesGameOver();


            }

        }
         if (Global.GameStop==false){
        if (NumberOfBox < 1)
            return;

        //The input force we are applying to the base of the tower.
        Vector3 input = new Vector3(Controller.RB.linearVelocity.x, 0f, 0f) * Time.fixedDeltaTime;

        //top wind force
        float topWindValue = Noise.Perlin1D(Time.time * topWindSpeed, 0.1f) * 2;  //-1 <->  1
        float topWindLimit = Mathf.Clamp(Mathf.Abs(topWindValue * 1.66f), 0.25f, 1.25f) - 0.25f;

        Vector3 topWind = Vector3.left * topWindValue * topWindLimit;
        topWind *= (topWindScale / 10);
        topWind *= (float)NumberOfBox / 5;  //less wind effect when short tower

        //Debug wind
        Debug.DrawRay(topPosition, topWind * 10, Color.blue);

        //Update a target point at the top of the tower
        topVel -= topVel * (topDrag / 10);

        Vector3 topDir = topPosition - transform.position;
        float topInstability = 1 - Vector3.Dot(Vector3.up, topDir.normalized);
        topInstability = Mathf.Clamp01(topInstability / topBalanceLoss);
        topInstability = Mathf.Max(0.2f, 1 - topInstability);   //First value is minimum force correction, 
        
        //TargetPos of top
        Vector3 topTarget = transform.position + towerOffset + boxOffset * (NumberOfBox-1);
        Vector3 topForce = topTarget - (topPosition + topVel * Mathf.Min(1, (float)(NumberOfBox-1)/5));
        //Y axis is not added to vel...
        topPosition += new Vector3(0, topForce.y, 0);
        //Remove y
        topForce.Scale(new Vector3(1, 0, 1));
        
        //Apply Scale
        topForce *= Mathf.Lerp(1, ((topForceScale) / NumberOfBox) * topInstability, (float)(NumberOfBox-1)/5);
        //Add to vel
        topVel += topForce + topWind * Time.fixedDeltaTime;
        //Update pos
        topPosition += topVel;
        //Lock Z axis
        topPosition.z = topTarget.z;


        Color towerDanger = Color.Lerp(Color.red, Color.green, topInstability);

        //Debut Overall Tower Stability
        Debug.DrawLine(topPosition + Vector3.up * 0.33f, topPosition - Vector3.up * 0.33f, towerDanger);
        Debug.DrawLine(topPosition + Vector3.right * 0.33f, topPosition - Vector3.right * 0.33f, towerDanger);
        Debug.DrawLine(topPosition + Vector3.forward * 0.33f, topPosition - Vector3.forward * 0.33f, towerDanger);
        Debug.DrawLine(topPosition, transform.position, towerDanger);
        
        //Set properties shared by all boxes
        Box.ConfigureBoxes(this);


        //For each box, top to bottom.
        //for (int i = boxes.Count - 1; i > -1; i--) {
        for(int i=0; i< boxes.Count; i++) { 
            //This box:
            Box b = boxes[i];

            //block below
            Vector3 posBelow = transform.position + towerOffset - boxOffset;
            if (i - 1 >= 0)
                posBelow = boxes[i - 1].position;

            //if (Box.V3NaN(posBelow)) //I don't know why this is NaN somethimes....  but this check will avoid errors.  I think it may be when 2 boxes share a position???
                //continue;

            //block above
            Vector3 posAbove = b.position + boxOffset;
            if (i + 1 < NumberOfBox)
                posAbove = boxes[i + 1].position;

            //Position in the tower, range bottom 0-1 top
            float heightFac = (float)i / NumberOfBox;

            //If the input matches the wind, then no force is added.  This is the game.
            Vector3 wind = ((Noise.Perlin2D(new Vector2(Time.time * difficultyWindSpeed, b.position.y / 10), 1) * Vector3.left) - input);
            //wind *= heightFac;  //Less wind at the bottom.   I think it's better without this.
            wind *= difficultyWindScale;  //Scale by difficulty
            
            //Update Position and Rotation
            b.SetPos(posBelow + boxOffset, wind);
            b.SetRot(posAbove, posBelow);

            //measure how close this box is to breaking, range safe 0-1 break            
            float dangerFactor = b.instability;

            //Mix a color based on the dangerFactor range safe green-red break
            Color dangerCol;

           if (!FeverEnabled){
               boxes[i].FeverEnabledbox=FeverEnabled;
            dangerCol = Color.Lerp(StableColor,InstableColor, dangerFactor);
           }
           else{
                boxes[i].FeverEnabledbox=FeverEnabled;
             dangerCol = FeverColor;
           }
             
            //FeverTextMat.color = boxes[boxes.Count-1].Mat.color;
            //Set the color of the box material
            boxes[i].Mat.color = dangerCol;

            //Do we need to break this box?  // Added UpdateZError To give a time before start calculating Z ERROR to prevent Bug .
            if (b.update && b.instability > 1 && b.updateZError) {
                Vector3 o = transform.position + towerOffset;  //Origin
                topPosition = o + (topPosition-o).normalized * (o - b.position).magnitude;
                topVel += new Vector3((o - topPosition).x, 0, 0).normalized * 0.1f;

                //Also break all the boxes above this one.
                if (i!=0)
       BreakAbove(i);

                UpdateScore();
            }
        }
    }
    }
private void BreakAbove(int i){
        Camera.main.GetComponent<Global>().animatewarning();
        int d = boxes.Count - 1;
                while (d >= i) {
                    //Change layer / enable pickup
                    boxes[d].obj.GetComponent<BoxBehaviour>().RemoveFromTower();
                    boxes[d].idx = -1;

                    //Remove from the list
                    boxes.RemoveAt(d);

                       Camera.main.GetComponent<CameraFollow>().ResetCameraPosition();
         Camera.main.GetComponent<Global>().ResetNumber();

                    //Lose points for breaking
                    score -= 5;
                    d--;
                }
}

    void OnTriggerEnter(Collider c) {
        BoxBehaviour bb = c.gameObject.GetComponent<BoxBehaviour>();
        if (bb == null)
            return;

        if (!bb.inTower && bb.gameObject.layer == Layers.OffTower) {

            if ( boxes.Count==0){
                 Camera.main.GetComponent<Global>().PlayerAnimator.SetInteger("CharAnimStat",2);
                 Debug.Log("DoneAnim");
            }
            //Where will this box end up?
            Vector3 startPoint = transform.position + towerOffset + boxOffset * NumberOfBox;

            //Create a new box
            Box b = new Box(startPoint, c.gameObject, NumberOfBox);

            if (NumberOfBox > 0)  //So we don't get an index out of range exception on the first box...
                b.vel = boxes[NumberOfBox - 1].vel;   //Give it some initial velocity


            //I think you added this line,   I would reccomend setting this in BoxBehaviour.AddToTower()  ->  TowerObj = t.gameobject
            //Rather than here.
            //c.gameObject.GetComponent<BoxBehaviour>().TowerObj = this.gameObject;

            //Setup the box...change layer, disable pickup, play spiral animation...
            bb.AddToTower(b, this);


            c.isTrigger = false; //Beccause instantiated test blocks are triggers to avoid self collision...
            
            //Add the box to the list
            boxes.Add(b);
           // MMVibrationManager.Haptic(HapticTypes.LightImpact);
            Camera.main.GetComponent<Global>().ResetNumber();
        
        // Adjust Camera
        Camera.main.GetComponent<CameraFollow>().ResetCameraPosition();

        }
    }
}

[System.Serializable]
public class Box {
    public int idx = -1;            //This boxes index in the list
    public bool update = false;
    public bool FeverEnabledbox;
    public bool updateZError=false;     //Should setPos update this box?
    public float yDefault;          //Default height
    public Vector3 vel, position;

    public float instability;       //Value that is used to break the tower

    public Rigidbody obj;           //This boxes object in the scene

    private Material _mat;          //Material for this box
    public Material Mat {           //Property to setup that material, happens when _mat is null
        get {
            if (_mat == null)
                //Have to make a copy, or all blocks will have same color
                obj.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = new Material( obj.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial);
            _mat = obj.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial;
            return _mat;
        }
    }

    //Constructor
    public Box(Vector3 p, GameObject o, int i) {
        position = p;
        yDefault = position.y;
        obj = o.GetComponent<Rigidbody>();
        obj.useGravity = false;
        idx = i;
    }

    private static int height;
    private static float forceScale, zSway, drag, stiffness, wobble,maxZerror,speed;
    private static Vector3 origin, topPosition, axis;
    private static AnimationCurve breakAngles;
    public static void ConfigureBoxes(TowerController t) {
        speed = t.Controller.Zspeed*Time.fixedDeltaTime;
        height = t.NumberOfBox;

        forceScale = Mathf.Lerp(1, t.forceScale, (float)(height-1)/5);
        zSway = t.zSway;
        drag = t.drag;
        stiffness = Mathf.Lerp(0, t.stiffness, (float)(height-1)/5);
        wobble = t.wobble;
        maxZerror = t.BreakDistanceZ;
        breakAngles = t.breakAngles;


        origin = t.transform.position + t.towerOffset;
        topPosition = t.topPosition;
        axis = topPosition - origin;
    }

    /// <summary>
    /// Algorithm to calculate the final position of the brick.
    /// </summary>
    public void SetPos(Vector3 target, Vector3 wind) {
        if (update)  //Before the box is at the top of the tower, position represents it place in the sim.  This way we arrive at the correct position.
            position = obj.transform.position;

        //Subtract drag from our velocity
        vel -= vel * drag;

        float idleOffset = 0;
        if(height > 0 && idx > 0)
            idleOffset = axis.magnitude * ((float)idx / (height -1));

        //Where is the default position of this box?
        Vector3 idlePosition =  origin + axis.normalized * idleOffset;

        //Calculate the force that would be required to move to the default position  (z axis)
        float zForce = (idlePosition - position).z;
        //Scale that force based on the boxes position in the tower.  Higher boxes get less force.
        Vector3 zVel = new Vector3(0, 0, zForce - (position.y / 50).ISmoothStart2() * zSway);

   Vector3 zSwayPos =idlePosition - new Vector3(0,0,(position.y/50).ISmoothStart2()*zSway);
   float zError = Mathf.Abs((zSwayPos-(position + Vector3.forward*speed)).z)/maxZerror;

        //A force that pulls the box up/down if a collision knocks the box out of place, but didn't break it.
        Vector3 yVel = new Vector3(0, (idlePosition - position).y * 0.6f, 0);
        
        //Calculate the force required to move this box to the previous box in the chain.
        Vector3 force = target - (position + (vel - (vel * wobble)));
        //Remove the y component of the vector. (flattem)
        force.Scale(new Vector3(1, 1, 0));

        //Calculate the force that would be required to move this box to the base  (x axis)  //This is stiffness
        Vector3 baseForce = idlePosition - (position + vel);// new Vector3(origin.x - position.x, 0, 0);
        baseForce.Scale(new Vector3(1, 1, 0));
                
        vel += force * stiffness.Invert();  //Add force * mix to the boxes velocity
        vel += baseForce * stiffness;  //Scale baseForce by the inverse of mix.

        //Imagine a line from the base of the tower to this box.
        Vector3 dir = (position + vel * forceScale) - (origin - Vector3.up * 2);
        dir.z = 0;

float ival = Vector3.Angle(dir.normalized,Vector3.up) /breakAngles.Evaluate(idx);

 ival+=zError;

float iFac = Mathf.Max(0,ival-0.25f);
 instability *=iFac*0.25f;

        //How well does the imagianry line match up?  range matches 1-0 perpundicular
      if (update && !FeverEnabledbox)
      instability +=ival;
      
      //  instability = Vector3.Angle(dir.normalized, Vector3.up) / breakAngles.Evaluate(idx);
        
        //Give the bottom boxes a little extra grace.  Otherwise it's easy to break with a sudden small movement.
        //instability *= (float)idx / 100;


        if (update) {
            Vector3 gravityForce = new Vector3(0, -2, 0);
            gravityForce *= ival *ival;

            vel += gravityForce;

            //If we can update, apply the forces to the rigidbody
            obj.linearVelocity = (vel * forceScale + yVel + zVel + wind) / Time.fixedDeltaTime;
            obj.angularVelocity = Vector3.zero;
        }
        else {
            //Otherwise update position.   The block was just picked up and is moving to the top.
            DebugAxis(position);
        }
        if (!FeverEnabledbox)
        position += vel * forceScale + yVel + zVel + wind + Vector3.forward*speed;
        else
        position = target;
    }

    private void DebugAxis(Vector3 p) {
        Debug.DrawLine(p + Vector3.up * 0.25f, p - Vector3.up * 0.25f, Color.green);
        Debug.DrawLine(p + Vector3.right * 0.25f, p - Vector3.right * 0.25f, Color.red);
        Debug.DrawLine(p + Vector3.forward * 0.25f, p - Vector3.forward * 0.25f, Color.blue);

    }

    //Sets the rotation of the box
    public void SetRot(Vector3 up, Vector3 down) {
        if (!update)
            return;

        //Line from previous to next box
        Vector3 dir = (up - down).normalized;
        Vector3 forward = Vector3.Cross(Vector3.left, dir).normalized;
        //Debug
        //Debug.DrawRay(down, dir, Color.magenta);

        //Use that line as the up axis for the box's rotation
        obj.transform.rotation = Quaternion.LookRotation(forward, dir.normalized + Vector3.up);
         obj.transform.rotation =Quaternion.Euler(0f,obj.transform.rotation.eulerAngles.y,obj.transform.rotation.eulerAngles.z);

       

    }
}

//Helper class to keep track to layers
public static class Layers {
    public const int OnTower = 8;
    public const int OffTower = 9;
    public const int Tower = 10;
    public const int Default = 0;
}
