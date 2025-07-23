using UnityEngine;
using AyoubInterpolaters;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BoxBehaviour : MonoBehaviour {
    public Box box;
    
    private bool stopupdating=false;
    //Box And Tower Relation Var
    public bool inTower = false;

    private bool TriggerAnimation=false;
    private Rigidbody _rigidbody;
    private Rigidbody RBody {
        get { 
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
    }

    [HideInInspector] float duration = 0.2f;
    //[HideInInspector] public GameObject Tower;
    private TowerController tower;

    //rotate Around player Var
    float randomRotationSpeed;

    //Quaternion FixBoxRot;

    private Vector3 startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;

    private void Start()
    {

        float randomScaleFactor = Random.Range(0f, 2f);
        if (randomScaleFactor >= 1f) {
        transform.localScale = new Vector3(transform.localScale.x*randomScaleFactor, transform.localScale.y, transform.localScale.z);
        transform.GetChild(2).transform.localScale = new Vector3(transform.GetChild(2).transform.localScale.x / randomScaleFactor, transform.GetChild(2).transform.localScale.y, transform.GetChild(2).transform.localScale.z);
    }
    }

    public void AddToTower(Box b, TowerController t) {
        //Debug.Break();
        inTower = true;
        box = b;
        tower = t;

        //Disable physics
        RBody.isKinematic = false;
        RBody.useGravity = false;
        RBody.linearDamping = 0;

        float height = box.position.y;

        //Save where we started...
        gameObject.layer = Layers.OnTower;
        startPosition = t.transform.InverseTransformPoint(transform.position);

        // Random  speed and diretion of the rotation 
        randomRotationSpeed = Random.Range(40f, 60f);
        if (Random.Range(0, 10) > 5)
            randomRotationSpeed *= -1;

        randomRotationSpeed *= height;

        float heightFac = Mathf.Min(1, height / 25);
        heightFac *= 0.8f;
        heightFac += 0.2f;

        //Start the timer:
        duration = 2f * heightFac.ISmoothStop2();
        Timer = duration;

        Destroy(transform.GetChild(2).gameObject);

    }

    public void RemoveFromTower() {
          Camera.main.GetComponent<Global>().FeverText.GetComponent<FeverController>().ResetFever();
        inTower = false;

        RBody.isKinematic = false;
        //RBody.velocity = box.vel;
        RBody.useGravity = true;
        
        gameObject.layer = Layers.Default;
        gameObject.GetComponent<BoxCollider>().material = null;
        //transform.parent = null;
        //box = null;

        // Adjust Camera
     
    }

    private float _timer = 0;
    private float Timer {
        get { return _timer; }
        set { _timer = value <= 0 ? 0 : value; }  //Never set a value less than 0
    }

    void Update()
    {
        if (stopupdating == false) { 
        if (box.idx == -1)
                return;

        if (Timer > 0)
        {
            Timer -= Time.deltaTime;

            float fac = 1 - (Timer / duration);

            //move to top.
            Vector3 bounce = Vector3.up * fac.ISmoothStart3().IArch() * 3;  //Move a little higher than the final position, then come down
            transform.position = Vector3.Lerp(tower.transform.TransformPoint(startPosition), box.position + bounce, fac);
            transform.rotation
 = startRotation;

            Vector3 rotPoint = tower.transform.position;

            if (tower.NumberOfBox > 0)
            {
                int yToIdx = Mathf.FloorToInt((transform.position.y - tower.boxes[0].position.y) / tower.boxOffset.y);

                if (yToIdx > 0 && yToIdx < tower.NumberOfBox)
                {
                    rotPoint = tower.boxes[yToIdx].position;

                    //Debug
                    Debug.DrawLine(tower.boxes[yToIdx].position + Vector3.up * 0.25f, tower.boxes[yToIdx].position - Vector3.up * 0.25f, Color.yellow);
                    Debug.DrawLine(tower.boxes[yToIdx].position + Vector3.right * 0.25f, tower.boxes[yToIdx].position - Vector3.right * 0.25f, Color.yellow);
                    Debug.DrawLine(tower.boxes[yToIdx].position + Vector3.forward * 0.25f, tower.boxes[yToIdx].position - Vector3.forward * 0.25f, Color.yellow);
                }
            }


            //Rotation around tower:
            if (Timer > 0.1f)
            {
                transform.RotateAround(rotPoint, Vector3.up, randomRotationSpeed * fac);
            }
            else
            {  //Rotation to final desired rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, tower.transform.rotation, fac);
            }
        }
        else
        {
            //We are in position now, the box can update and sway itself
            box.update = true;
            if (TriggerAnimation == false)
            {
                   
                    stopupdating = true;
                TriggerAnimation = true;
                StartCoroutine("TriggerAnimationCo");
                StartCoroutine("StartUpdateZerror");
            }
        }
    }
    }

    IEnumerator StartUpdateZerror(){
        yield return new WaitForSecondsRealtime(0.3f);
        box.updateZError=true;
    }
     IEnumerator TriggerAnimationCo(){
         Camera.main.GetComponent<Global>().FeverText.GetComponent<FeverController>().IncrementFever();
         transform.GetChild(1).transform.DOScale(0.65f,0.3f);
         transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0f,0.5f);
         return null;
     }
    /*
    private void OnCollisionStay(Collision col) {
        Debug.Log("incollision");
        if (col.collider.gameObject.layer == Layers.OffTower) {
            Debug.Log("incollision");
            box.vel += col.impulse;
        }
    }
    */
}
