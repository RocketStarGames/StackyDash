using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject Slider;
    private bool Done=false;


void OnMouseDown(){
    if (Done==false){
        Done = true;
    transform.parent.GetComponent<Global>().StartGame();
    Slider.GetComponent<HowToPlay>().KillHTP();
    }
}
}
