using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{


 private bool NewLevelSpawned=false;
 private GameObject LevelTrigger;

 void Start(){

// Spawn Next Level to This One . 
  Camera.main.GetComponent<Global>().SpawnNewLevel(this.gameObject);
          NewLevelSpawned=true;

 }


}
