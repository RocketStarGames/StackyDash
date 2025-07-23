using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InstantiateHeights : MonoBehaviour
{
   
   public GameObject HeightsObj;
 public int DefaultFontSize;

 public GameObject CamTrigger;
 private bool Done;
public GameObject TruckPosition;
public GameObject TruckPrefab;

  public int IntNumberOfTrucks ;
    void Start()
    {
      CamTrigger = Camera.main.transform.GetChild(0).transform.gameObject;
      
  float yOffset = transform.localScale.y;
        // Lets say 5 Box are equal to 5 meters .
         int numberOfLevel =	(int)(Camera.main.GetComponent<CameraFollow>().BoxNumber/5);
         DefaultFontSize =transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().fontSize;
        for (int i=1;i<=30;i++){
          
            
GameObject HObj = Instantiate(HeightsObj,new Vector3(transform.position.x,transform.position.y+(yOffset*i),transform.position.z),transform.rotation);
  HObj.GetComponent<Renderer>().sharedMaterial = new Material(gameObject.GetComponent<Renderer>().sharedMaterial);
  HObj.transform.parent=  gameObject.transform;
 HObj.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = (5*i).ToString() +" M";

DefaultFontSize+=1;
  HObj.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().fontSize = DefaultFontSize ;
      }
        
   
        
    }

    public void MoveBarier(){
      transform.GetChild(2).transform.DOLocalMoveY(-60f,1.5f);
    }

    public void Activatetrucks(int AmountOfBox){
       GameObject Trucks= Instantiate(TruckPrefab,TruckPosition.transform.position,Quaternion.identity);
           float NumberOfTrucks ;
          
           NumberOfTrucks = (AmountOfBox/Trucks.GetComponent<TruckController>().MaxBoxSlotsByTruck);
             if (AmountOfBox%Trucks.GetComponent<TruckController>().MaxBoxSlotsByTruck!=0){
                       IntNumberOfTrucks = (int)NumberOfTrucks+1;
             }
             else
             {
                 IntNumberOfTrucks = (int)NumberOfTrucks;
             }
             for (int i=0;i<IntNumberOfTrucks;i++){
             Trucks.transform.GetChild(i).gameObject.SetActive(true);
             }

Camera.main.GetComponent<Global>().Trucks = Trucks;
      //  Destroy(TruckPosition);
    }

}
