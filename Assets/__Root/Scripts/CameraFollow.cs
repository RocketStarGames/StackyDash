using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraFollow : MonoBehaviour {

public float BoxNumber;
    public Transform FollowedObject;
    public float Yoffset;
    public float Zoffset;
    public float XDefaultoffset;
    
  
  public bool enableFollow=true;

    public float CameraFactor;
    public float newxoffset;
    public float newyoffset;
    public float newzoffset;
    private float DefaultYoffset;
    private float DefaultZoffset;
    public GameObject OurTower;
    public float Smoothness;
public float RotationSmoothness;
public float Xoffset;
public float XSmoothness;

    public bool StopFollowing = false;

    public Quaternion CurrentRotation;
    public Vector3 NewPos;
    public Quaternion NewRot;

    // Use this for initialization
    void Start() {

        NewRot = transform.rotation;
        Application.targetFrameRate = 60;
        newyoffset = Yoffset;
        newzoffset = Zoffset;
        DefaultYoffset = Yoffset;
        DefaultZoffset = Zoffset;
ResetCameraPosition();
    }

    [System.Serializable]
    public class KeyFrameCam {


        public float CurrentHeight;
        public Vector3 CamPosForThisFrame;
        public Quaternion CamRotationForThisFrame;

        public KeyFrameCam(float H, Vector3 PosFrame, Quaternion RotationFrame) {
            //this is an empty constructor i could have intialized values here .
        }

    }

    public List<KeyFrameCam> OurFrameReferences = new List<KeyFrameCam>();


    public void ResetCameraPosition() {
        float height = OurTower.GetComponent<TowerController>().NumberOfBox+1;
        BoxNumber = height;

        if (height <= 45) {
            
            CameraFactor = height / OurFrameReferences[1].CurrentHeight;
            NewPos = Vector3.Lerp(OurFrameReferences[0].CamPosForThisFrame, OurFrameReferences[1].CamPosForThisFrame, CameraFactor);
            NewRot = Quaternion.Lerp(OurFrameReferences[0].CamRotationForThisFrame, OurFrameReferences[1].CamRotationForThisFrame, CameraFactor);
        }
        else if (height > 45)
        {
            float zeroedVal = OurFrameReferences[2].CurrentHeight - OurFrameReferences[1].CurrentHeight;

            CameraFactor = (height - OurFrameReferences[1].CurrentHeight) / zeroedVal;
            NewPos = Vector3.Lerp(OurFrameReferences[1].CamPosForThisFrame, OurFrameReferences[2].CamPosForThisFrame, CameraFactor);
            NewRot = Quaternion.Lerp(OurFrameReferences[1].CamRotationForThisFrame, OurFrameReferences[2].CamRotationForThisFrame, CameraFactor);
        }

        newyoffset = NewPos.y;
        newzoffset = NewPos.z;
    }

    public void ResetDefaultCameraPos() {
        newyoffset = DefaultYoffset;
        newzoffset = DefaultZoffset;
    }
    
    void FixedUpdate() {

        // XFollow 

        CurrentRotation = transform.rotation;
        // Xoffset
        if (enableFollow==true){
        	float SmoothedXAxis = Mathf.Lerp (transform.position.x, FollowedObject.position.x+XDefaultoffset, XSmoothness);
			float ClampedSmoothedXAxis = Mathf.Clamp (SmoothedXAxis, -Xoffset+XDefaultoffset, Xoffset+XDefaultoffset);
			
        Vector3 TCP = new Vector3(transform.position.x, Yoffset, FollowedObject.position.z + Zoffset);
        Vector3 SmoothedTCP = Vector3.Lerp(transform.position, TCP, Smoothness);

        transform.position =new Vector3(ClampedSmoothedXAxis, SmoothedTCP.y,SmoothedTCP.z);
               
        transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, RotationSmoothness);
        if (Yoffset != newyoffset) {
            Yoffset = Mathf.Lerp(Yoffset, newyoffset, Time.fixedDeltaTime * 2f);
            Zoffset = Mathf.Lerp(Zoffset, newzoffset - 5, Time.fixedDeltaTime * 2f);
        }
        }

        /*   Quaternion newRot = Quaternion.Lerp(OurFrameReferences[0].CamRotationForThisFrame,OurFrameReferences[1].CamRotationForThisFrame,CameraFactor);
          transform.rotation = newRot;
    */

    }

}
