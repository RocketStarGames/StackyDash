﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleOnBoxBehaviour : MonoBehaviour
{

void Update(){

    transform.Rotate(Vector3.up, 80 * Time.deltaTime);
}

}
